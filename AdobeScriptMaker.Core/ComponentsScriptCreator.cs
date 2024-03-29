﻿using AdobeComponents.Animation;
using AdobeComponents.CommonValues;
using AdobeComponents.Components;
using AdobeComponents.Components.Layers;
using AdobeComponents.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdobeScriptMaker.Core
{
    public class ComponentsScriptCreator
    {
        private readonly ScriptBuilder _scriptBuilder = new ScriptBuilder();
        public string Visit(AdobeScript script, params SharedControlValue[] sharedControlValues)
        {
            foreach (var composition in script.Compositions)
                VisitComposition(composition);

            return _scriptBuilder.GetScriptText(sharedControlValues ?? Array.Empty<SharedControlValue>());
        }

        private void VisitComposition(AdobeComposition composition)
        {
            var compositionRef = "app.project.activeItem";

            var nullLayerVar = _scriptBuilder.GetNextAutoVariable();
            _scriptBuilder.AddText($@"var {nullLayerVar} = {compositionRef}.layers.addNull();
{nullLayerVar}.position.setValue([0,0]);");

            foreach (var layer in composition.Layers)
                VisitLayer(compositionRef, layer, nullLayerVar);
        }

        private void VisitLayer(string compositionRef, AdobeLayer layer, string nullLayerVar)
        {
            foreach (var drawing in layer.Drawings)
            {
                var layerVar = _scriptBuilder.GetNextAutoVariable();

                if (drawing is AdobeTextComponent textComponent)
                {
                    //Make sure to add the text document to the layer before setting properties on the layer
                    //otherwise a runtime exception will be thrown by adobe
                    //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addtext
                    _scriptBuilder.AddText($"var {layerVar} = {compositionRef}.layers.addText('{textComponent.TextValue}')");
                }
                else if (drawing is AdobeTextControl textControl)
                    _scriptBuilder.AddText($"var {layerVar} = {compositionRef}.layers.addText()");
                else
                    _scriptBuilder.AddText($"var {layerVar} = {compositionRef}.layers.addShape()");

                if (layer.InPoint != null)
                    _scriptBuilder.AddText($"{layerVar}.inPoint = {layer.InPoint};");

                if (layer.OutPoint != null)
                    _scriptBuilder.AddText($"{layerVar}.outPoint = {layer.OutPoint};");

                _scriptBuilder.AddText($"{layerVar}.parent = {nullLayerVar};");

                if (drawing is AdobePathGroupComponent pathGroup)
                    VisitPathGroup(layerVar, pathGroup);
                else if (drawing is AdobeTextComponent text)
                    VisitText(layerVar, text);
                else if (drawing is AdobeSliderControl slider)
                    VisitSlider(slider);
                else if (drawing is AdobeSharedColorControl sharedColor)
                    VisitColorControl(sharedColor);
                else if (drawing is AdobeTextControl textControl)
                    VisitText(layerVar, textControl);
                else
                    throw new NotSupportedException(drawing.GetType().FullName);

                AddEffects(drawing, layerVar);
            }
        }

        private void AddEffects(object drawing, string layerVar)
        {
            if (drawing is IAdobeSupportsMaskComponent maskComponent && maskComponent.Mask != null)
                VisitMask(layerVar, maskComponent.Mask);

            if (drawing is IAdobeSupportsScribbleEffect scribbleComponent && scribbleComponent.ScribbleEffect != null)
                VisitScribbleEffect(layerVar, scribbleComponent.ScribbleEffect);

            if (drawing is IAdobeSupportsTrimPathsEffect trimPathsComponent && trimPathsComponent.TrimPathsEffect != null)
            {
                var trimPathsVar = _scriptBuilder.GetNextAutoVariable();
                var trimPathsText = @$"var {trimPathsVar} = {layerVar}.property('ADBE Root Vectors Group').addProperty('ADBE Vector Filter - Trim');";

                trimPathsText = AddSetPropertyScript($"{trimPathsVar}.property('Start')", trimPathsText, trimPathsComponent.TrimPathsEffect.Start);
                trimPathsText = AddSetPropertyScript($"{trimPathsVar}.property('End')", trimPathsText, trimPathsComponent.TrimPathsEffect.End);

                _scriptBuilder.AddText(trimPathsText);
            }
        }

        private void VisitPathGroup(string layerVar, AdobePathGroupComponent pathGroup)
        {
            foreach (var path in pathGroup.Paths)
            {
                VisitPath(layerVar, path);
                AddEffects(path, layerVar);
            }
        }

        private void VisitPath(string layerVar, AdobePathComponent path)
        {
            var baseGroupVar = _scriptBuilder.GetNextAutoVariable();
            var vectorsGroupVar = _scriptBuilder.GetNextAutoVariable();
            var vectorGroupVar = _scriptBuilder.GetNextAutoVariable();
            var strokeVar = _scriptBuilder.GetNextAutoVariable();

            var scriptText = $@"var {baseGroupVar} = {layerVar}.property('Contents').addProperty('ADBE Vector Group');
var {vectorsGroupVar} = {baseGroupVar}.addProperty('ADBE Vectors Group');
var {vectorGroupVar} = {vectorsGroupVar}.addProperty('ADBE Vector Shape - Group')
{CreateSetVerticesCode(path.Points, vectorGroupVar, path.IsClosed)}
var {strokeVar} = {vectorsGroupVar}.addProperty('ADBE Vector Graphic - Stroke');
{strokeVar}.property('ADBE Vector Stroke Width').setValue('{path.Thickness}');
{layerVar}.property('Transform').property('Position').setValue([0, 0]);";

            if (path.ColorValue != null)
                scriptText = string.Join(Environment.NewLine, scriptText, $"{strokeVar}.property('ADBE Vector Stroke Color'){path.ColorValue.GetScriptText()};");
            else
                scriptText = string.Join(Environment.NewLine, scriptText, $"{strokeVar}.property('ADBE Vector Stroke Color').setValue([0, 0, 0]);");

            if (path.StrokeWidth is AdobeSliderValue sliderVal)
                scriptText = string.Join(Environment.NewLine, scriptText, $"{strokeVar}.property('ADBE Vector Stroke Width').setValue({sliderVal.Value});");
            else
                scriptText = string.Join(Environment.NewLine, scriptText, $"{strokeVar}.property('ADBE Vector Stroke Width').expression = \"{path.StrokeWidth.GetScriptText()}\";");

            _scriptBuilder.AddText(scriptText);
        }

        private void VisitText(string layerVar, AdobeTextControl text)
        {
            var adobeIndex = 1;
            foreach (var value in text.Values)
            {
                _scriptBuilder.AddText($"{layerVar}.property('ADBE Text Properties').property('ADBE Text Document').setValueAtTime({value.Time}, '{value.Value}');");
                _scriptBuilder.AddText($"{layerVar}.property('ADBE Text Properties').property('ADBE Text Document').setInterpolationTypeAtKey({adobeIndex}, KeyframeInterpolationType.HOLD, KeyframeInterpolationType.HOLD);");
            }
        }

        private void VisitColorControl(AdobeSharedColorControl colorControl)
        {
            var colorControlVar = _scriptBuilder.GetNextAutoVariable();

            var sharedLayersVar = _scriptBuilder.GetSharedControlsLayerVar();
            _scriptBuilder.AddText(@$"var {colorControlVar} = {sharedLayersVar}.effect.addProperty('ADBE Color Control');
{colorControlVar}.name = '{colorControl.ControlName}';");
        }

        private void VisitSlider(AdobeSliderControl slider)
        {
            var sliderControlVar = _scriptBuilder.GetNextAutoVariable();
            var sliderVar = _scriptBuilder.GetNextAutoVariable();
            var sharedLayersVar = _scriptBuilder.GetSharedControlsLayerVar();

            _scriptBuilder.AddText(@$"var {sliderControlVar} = {sharedLayersVar}.effect.addProperty('ADBE Slider Control');
var {sliderVar} = {sliderControlVar}('Slider');");

            var adobeIndex = 1;
            foreach (var value in slider.Values)
            {
                _scriptBuilder.AddText($"{sliderVar}.setValueAtTime({value.Time}, {value.Value});");
                _scriptBuilder.AddText($"{sliderVar}.setInterpolationTypeAtKey({adobeIndex}, KeyframeInterpolationType.HOLD, KeyframeInterpolationType.HOLD);");
            }

            if (!string.IsNullOrEmpty(slider.Name))
                _scriptBuilder.AddText($"{sliderControlVar}.name = '{slider.Name}';");
        }

        private void VisitMask(string layerVar, AdobeMaskComponent mask)
        {
            var maskVar = _scriptBuilder.GetNextAutoVariable();
            var maskShapeVar = _scriptBuilder.GetNextAutoVariable();

            var scriptText = $@"var {maskVar} = {layerVar}.Masks.addProperty('Mask');
{maskVar}.inverted = {mask.IsInverted.ToString().ToLower()}
var {maskShapeVar} = {maskVar}.property('maskShape');
{CreateSetVerticesCode(mask.PathComponent.Points, maskShapeVar, mask.PathComponent.IsClosed, true)}";

            if (!string.IsNullOrEmpty(mask.MaskName))
                scriptText = string.Join(Environment.NewLine, scriptText, $"{maskVar}.name = '{mask.MaskName}';");

            _scriptBuilder.AddText(scriptText);
        }

        private void VisitScribbleEffect(string layerVar, AdobeScribbleEffect scribbleEffect)
        {
            var scribbleVar = _scriptBuilder.GetNextAutoVariable();

            var scriptText = $@"var {scribbleVar} = {layerVar}.Effects.addProperty('ADBE Scribble Fill');
{scribbleVar}.Mask = '{scribbleEffect.MaskName}';";

            if (scribbleEffect.ColorValue != null)
            {
                scriptText = string.Join(Environment.NewLine, scriptText, $"{scribbleVar}.Color{scribbleEffect.ColorValue.GetScriptText()};");
            }

            scriptText = AddSetPropertyScript($"{scribbleVar}.property('Start')", scriptText, scribbleEffect.Start);
            scriptText = AddSetPropertyScript($"{scribbleVar}.property('End')", scriptText, scribbleEffect.End);

            if (scribbleEffect.WigglesPerSecond is AdobeSliderValue wigglesSliderVal)
            {
                scriptText = string.Join(Environment.NewLine, scriptText, $"{scribbleVar}.property('Wiggles/Second').setValue({wigglesSliderVal.Value});");
            }
            else
            {
                scriptText = string.Join(Environment.NewLine, scriptText, $"{scribbleVar}.property('Wiggles/Second').expression = \"{scribbleEffect.WigglesPerSecond.GetScriptText()}\";");
            }

            _scriptBuilder.AddText(scriptText);
        }

        private void VisitText(string layerVar, AdobeTextComponent text)
        {
            var lines = new List<string>();

            //https://ae-scripting.docsforadobe.dev/other/textdocument.html?highlight=TextDocument#textdocument
            var textDocVar = _scriptBuilder.GetNextAutoVariable();
            lines.Add($"var {textDocVar} = new TextDocument('{text.TextValue}');");

            var additionalXOffset = text.Justification == AdobeTextJustification.Right
                ? text.Size.Width
                : 0;

            var fontHeightCorrection = GetFontHeightCorrection(text.Size.Height);
            if (text.Left is AdobeSliderValue leftValue && text.Top is AdobeSliderValue topValue)
            {
                lines.Add($"{layerVar}.position.setValue([{leftValue.Value + additionalXOffset}, {topValue.Value + text.Size.Height - fontHeightCorrection}]);");
            }
            else
            {
                var leftPositionText = $"{text.Left.GetScriptText()} + {additionalXOffset}";
                var topPositionText = $"{text.Top.GetScriptText()} + {text.Size.Height} - {fontHeightCorrection}";

                lines.Add($"{layerVar}.position.expression = \"[({leftPositionText}), ({topPositionText})]\";");
            }

            //The source text needs to be saved and then reset or else it doesn't work, which is weird. The idea was taken from:
            //https://community.adobe.com/t5/after-effects-discussions/unable-to-execute-script-at-line-17-unable-to-set-value-as-it-is-not-associated-with-a-layer/td-p/11782185
            var sourceTextVar = _scriptBuilder.GetNextAutoVariable();
            lines.Add(@$"var {sourceTextVar} = {layerVar}.text.sourceText;
var {textDocVar} = {sourceTextVar}.value;
{textDocVar}.font = '{text.TextSettings.FontName}';
{textDocVar}.fontSize = {text.TextSettings.FontSizeInPixels};
{textDocVar}.justification = {(text.Justification == AdobeTextJustification.Right ? "ParagraphJustification.RIGHT_JUSTIFY" : "ParagraphJustification.LEFT_JUSTIFY")};");

            if (text.FontColor != null)
            {
                var fillVar = _scriptBuilder.GetNextAutoVariable();
                lines.Add($@"var {fillVar} = {layerVar}.Effects.addProperty('ADBE Fill');");

                if (text.FontColor != null)
                {
                    lines.Add($"{fillVar}.Color{text.FontColor.GetScriptText()};");
                }
            }
            else
                lines.Add($"{textDocVar}.fillColor.setValue([1, 0, 0]);");

            lines.Add($"{sourceTextVar}.setValue({ textDocVar});");

            _scriptBuilder.AddText(string.Join(Environment.NewLine, lines.ToArray()));
        }

        private float GetFontHeightCorrection(float height)
        {
            return height * 0.15f;
        }

        private string CreateSetVerticesCode(IAnimatedValue<PointF[]> points,
            string vectorGroupVar,
            bool isClosed,
            bool isMask = false)
        {
            if (!points.IsAnimated)
                return CreateShape(points.GetValues().Single().Value, isClosed, vectorGroupVar, null, isMask);
            else
            {
                return String.Join(Environment.NewLine,
                    points.GetValues()
                        .Select(x => CreateShape(x.Value, isClosed, vectorGroupVar, x.Time.Time, isMask)));
            }
        }

        private string CreateShape(IEnumerable<PointF> points,
            bool isClosed,
            string vectorGroupVar,
            double? time,
            bool isMask = false)
        {
            var shapeVar = _scriptBuilder.GetNextAutoVariable();

            var result = $@"var {shapeVar} = new Shape();
{shapeVar}.vertices = {ConvertPointsToJavascriptArg(points)};
{shapeVar}.closed = {isClosed.ToString().ToLower()};";

            if (time == null)
                result = string.Join(Environment.NewLine, result, $"{vectorGroupVar}{(isMask ? "" : ".property('Path')")}.setValue({shapeVar});");
            else
                result = string.Join(Environment.NewLine, result, $"{vectorGroupVar}{(isMask ? "" : ".property('Path')")}.setValueAtTime({time}, {shapeVar});");

            return result;

        }

        private string ConvertPointsToJavascriptArg(IEnumerable<PointF> points)
        {
            var pointArgs = string.Join(",", points.Select(x => $"[{x.X},{x.Y}]"));
            return $"[{pointArgs}]";
        }

        private string AddSetPropertyScript<T>(string propertyExpression, string scriptText, IAnimatedValue<T> value)
        {
            if (value != null)
            {
                string additionalText;
                if (!value.IsAnimated)
                    additionalText = $"{propertyExpression}.setValue({value.GetValues().Single().Value});";
                else
                {
                    additionalText = string.Join(Environment.NewLine, value.GetValues()
                        .Select(x => $"{propertyExpression}.setValueAtTime({x.Time.Time}, {x.Value});"));
                }

                return String.Join(Environment.NewLine, scriptText, additionalText);
            }
            else
                return scriptText;
        }
    }

    public class ScriptBuilder
    {
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly ScriptContext _context = new ScriptContext();

        private const string SHARED_CONTROLS_LAYER_NAME = "Shared Controls Layer";
        private string _sharedControlsLayerVar = null;

        private string _compositionRef = "app.project.activeItem";

        public string GetNextAutoVariable()
        {
            return _context.GetNextAutoVariable();
        }

        public void AddText(string text)
        {
            _builder.AppendLine(text);
        }

        public string GetScriptText(SharedControlValue[] sharedControlValues)
        {
            var scriptText = _builder.ToString();
            if (_sharedControlsLayerVar != null)
            {
                var sharedControlsLayerScriptText = @$"var {_sharedControlsLayerVar} = {_compositionRef}.layers.addNull();
{_sharedControlsLayerVar}.adjustmentLayer = true;
{_sharedControlsLayerVar}.name = '{SHARED_CONTROLS_LAYER_NAME}';";

                var sharedControlsValues = "";
                foreach (var sharedControlValue in sharedControlValues)
                {
                    sharedControlsValues = String.Join(Environment.NewLine, sharedControlsValues, $"{_sharedControlsLayerVar}.Effects.property('{sharedControlValue.ControlName}'){sharedControlValue.PropertyText}.setValue({sharedControlValue.Value});");
                }

                scriptText = String.Join(Environment.NewLine, sharedControlsLayerScriptText, scriptText, sharedControlsValues);
            }

            return scriptText;
        }

        public string GetSharedControlsLayerVar()
        {
            if (_sharedControlsLayerVar == null)
                _sharedControlsLayerVar = _context.GetNextAutoVariable();

            return _sharedControlsLayerVar;
        }
    }

    public class SharedControlValue
    {
        public readonly string ControlName;
        public readonly string Value;

        public string PropertyText = ".Color";

        public SharedControlValue(string controlName,
            string value)
        {
            ControlName = controlName;
            Value = value;
        }
    }
}
