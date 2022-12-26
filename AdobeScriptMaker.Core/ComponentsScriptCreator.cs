using AdobeComponents.Animation;
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
        public string Visit(AdobeScript script)
        {
            foreach (var composition in script.Compositions)
                VisitComposition(composition);

            return _scriptBuilder.GetScriptText();
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
            var textDrawings = layer.Drawings.OfType<AdobeTextControl>().ToList();
            var otherDrawings = layer.Drawings.Where(x => !(x is AdobeTextControl)).ToList();

            if (otherDrawings.Any())
            {
                var layerVar = _scriptBuilder.GetNextAutoVariable();

                if (otherDrawings.Count == 1 && otherDrawings.First() is AdobeTextComponent textComponent)
                {
                    //Make sure to add the text document to the layer before setting properties on the layer
                    //otherwise a runtime exception will be thrown by adobe
                    //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addtext
                    _scriptBuilder.AddText($"var {layerVar} = {compositionRef}.layers.addText('{textComponent.TextValue}')");
                }
                else
                    _scriptBuilder.AddText($"var {layerVar} = {compositionRef}.layers.addShape()");

                if (layer.InPoint != null)
                    _scriptBuilder.AddText($"{layerVar}.inPoint = {layer.InPoint};");

                if (layer.OutPoint != null)
                    _scriptBuilder.AddText($"{layerVar}.outPoint = {layer.OutPoint};");

                _scriptBuilder.AddText($"{layerVar}.parent = {nullLayerVar};");

                foreach (var drawing in otherDrawings)
                {
                    if (drawing is AdobePathComponent path)
                        VisitPath(layerVar, path);
                    else if (drawing is AdobeMaskComponent mask)
                        VisitMask(layerVar, mask);
                    else if (drawing is AdobeTextComponent text)
                        VisitText(layerVar, text);
                    else if (drawing is AdobeSliderControl slider)
                        VisitSlider(compositionRef, slider);
                    else if (drawing is AdobeSharedColorControl sharedColor)
                        VisitColorControl(sharedColor);
                    else if (drawing is AdobeScribbleEffect scribble)
                        VisitScribbleEffect(layerVar, scribble);
                    else
                        throw new NotSupportedException(drawing.GetType().FullName);
                }
            }

            if (textDrawings.Any())
            {
                var layerVar = _scriptBuilder.GetNextAutoVariable();
                _scriptBuilder.AddText($"var {layerVar} = {compositionRef}.layers.addText()");

                if (layer.InPoint != null)
                    _scriptBuilder.AddText($"{layerVar}.inPoint = {layer.InPoint};");

                if (layer.OutPoint != null)
                    _scriptBuilder.AddText($"{layerVar}.outPoint = {layer.OutPoint};");

                _scriptBuilder.AddText($"{layerVar}.parent = {nullLayerVar};");

                foreach (var drawing in textDrawings)
                {
                    if (drawing is AdobeTextControl text)
                        VisitText(layerVar, text);
                    else
                        throw new NotSupportedException(drawing.GetType().FullName);
                }
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
                scriptText = string.Join(Environment.NewLine, scriptText, $"{strokeVar}.property('ADBE Vector Stroke Color').setValue([0, 0, 0])");

            if (path.TrimPaths != null)
            {
                var trimPathsVar = _scriptBuilder.GetNextAutoVariable();
                scriptText = string.Join(Environment.NewLine, scriptText, @$"var {trimPathsVar} = {layerVar}.property('ADBE Root Vectors Group').addProperty('ADBE Vector Filter - Trim');");

                scriptText = AddSetPropertyScript($"{trimPathsVar}.property('Start')", scriptText, path.TrimPaths.Start);
                scriptText = AddSetPropertyScript($"{trimPathsVar}.property('End')", scriptText, path.TrimPaths.End);
            }

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

        private void VisitSlider(string compositionRef, AdobeSliderControl slider)
        {
            var nullLayerVar = _scriptBuilder.GetNextAutoVariable();

            _scriptBuilder.AddText(@$"var {nullLayerVar} = {compositionRef}.layers.addNull();
{nullLayerVar}.effect.addProperty('ADBE Slider Control')('Slider');");

            var adobeIndex = 1;
            foreach (var value in slider.Values)
            {
                _scriptBuilder.AddText($"{nullLayerVar}.effect('Slider Control').property('Slider').setValueAtTime({value.Time}, {value.Value});");
                _scriptBuilder.AddText($"{nullLayerVar}.effect('Slider Control').property('Slider').setInterpolationTypeAtKey({adobeIndex}, KeyframeInterpolationType.HOLD, KeyframeInterpolationType.HOLD);");
            }

            if (!string.IsNullOrEmpty(slider.Name))
                _scriptBuilder.AddText($"{nullLayerVar}.name = { slider.Name};");
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

            _scriptBuilder.AddText(scriptText);
        }

        private void VisitText(string layerVar, AdobeTextComponent text)
        {
            var lines = new List<string>();

            //https://ae-scripting.docsforadobe.dev/other/textdocument.html?highlight=TextDocument#textdocument
            var textDocVar = _scriptBuilder.GetNextAutoVariable();
            lines.Add($"var {textDocVar} = new TextDocument('{text.TextValue}');");

            lines.Add($"{layerVar}.position.setValue([{text.Bounds.Left + text.Bounds.Width}, {text.Bounds.Top + text.Bounds.Height - GetFontHeightCorrection(text.Bounds.Height)}]);");

            //The source text needs to be saved and then reset or else it doesn't work, which is weird. The idea was taken from:
            //https://community.adobe.com/t5/after-effects-discussions/unable-to-execute-script-at-line-17-unable-to-set-value-as-it-is-not-associated-with-a-layer/td-p/11782185
            var sourceTextVar = _scriptBuilder.GetNextAutoVariable();
            lines.Add(@$"var {sourceTextVar} = {layerVar}.text.sourceText;
var {textDocVar} = {sourceTextVar}.value;
{textDocVar}.font = '{text.TextSettings.FontName}';
{textDocVar}.fontSize = {text.TextSettings.FontSizeInPixels};
{textDocVar}.justification = ParagraphJustification.RIGHT_JUSTIFY;
{sourceTextVar}.setValue({textDocVar});");

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

        public string GetScriptText()
        {
            var scriptText = _builder.ToString();
            if (_sharedControlsLayerVar != null)
            {
                var sharedControlsLayerScriptText = @$"var {_sharedControlsLayerVar} = {_compositionRef}.layers.addNull();
{_sharedControlsLayerVar}.adjustmentLayer = true;
{_sharedControlsLayerVar}.name = '{SHARED_CONTROLS_LAYER_NAME}';";

                scriptText = String.Join(Environment.NewLine, sharedControlsLayerScriptText, scriptText);
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
}
