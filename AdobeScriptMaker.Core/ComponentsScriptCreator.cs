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
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly ScriptContext _context = new ScriptContext();

        public string Visit(AdobeScript script)
        {
            foreach (var composition in script.Compositions)
                VisitComposition(composition);

            return _builder.ToString();
        }

        private void VisitComposition(AdobeComposition composition)
        {
            var compositionRef = "app.project.activeItem";

            var nullLayerVar = _context.GetNextAutoVariable();
            _builder.AppendLine($@"var {nullLayerVar} = {compositionRef}.layers.addNull();
{nullLayerVar}.position.setValue([0,0]);");

            var sharedControlsLayerName = "Shared Controls Layer";
            string sharedControlsLayerVar = null;

            var savedLayers = new List<AdobeLayer>();
            foreach (var layer in composition.Layers)
            {
                if (layer.Drawings.OfType<IAdobeSharedValueControl>().Any())
                    savedLayers.Add(layer);
                else
                    VisitLayer(compositionRef, layer, nullLayerVar, sharedControlsLayerName, ref sharedControlsLayerVar);
            }

            //Write out the shared controls last so that they appear at the top
            foreach (var savedLayer in savedLayers)
                VisitLayer(compositionRef, savedLayer, nullLayerVar, sharedControlsLayerName, ref sharedControlsLayerVar);
        }

        private void VisitLayer(string compositionRef, AdobeLayer layer, string nullLayerVar, string sharedControlsLayerName, ref string sharedControlsLayerVar)
        {
            var textDrawings = layer.Drawings.OfType<AdobeTextControl>().ToList();
            var otherDrawings = layer.Drawings.Where(x => !(x is AdobeTextControl)).ToList();

            if (otherDrawings.Any())
            {
                var layerVar = _context.GetNextAutoVariable();
                _builder.AppendLine($"var {layerVar} = {compositionRef}.layers.addShape()");

                if (layer.InPoint != null)
                    _builder.AppendLine($"{layerVar}.inPoint = {layer.InPoint};");

                if (layer.OutPoint != null)
                    _builder.AppendLine($"{layerVar}.outPoint = {layer.OutPoint};");

                _builder.AppendLine($"{layerVar}.parent = {nullLayerVar};");

                foreach (var drawing in otherDrawings)
                {
                    if (drawing is IAdobeSharedValueControl)
                    {
                        if (sharedControlsLayerVar == null)
                        {
                            sharedControlsLayerVar = _context.GetNextAutoVariable();
                            _builder.AppendLine(@$"var {sharedControlsLayerVar} = { compositionRef}.layers.addNull();
{sharedControlsLayerVar}.adjustmentLayer = true;
{sharedControlsLayerVar}.name = '{sharedControlsLayerName}';");
                        }
                    }

                    if (drawing is AdobePathComponent path)
                        VisitPath(layerVar, path);
                    else if (drawing is AdobeMaskComponent mask)
                        VisitMask(layerVar, mask);
                    else if (drawing is AdobeTextComponent text)
                        VisitText(compositionRef, text);
                    else if (drawing is AdobeSliderControl slider)
                        VisitSlider(compositionRef, slider);
                    else if (drawing is AdobeSharedColorControl sharedColor)
                        VisitColorControl(sharedControlsLayerVar, sharedColor);
                    else if (drawing is AdobeScribbleEffect scribble)
                        VisitScribbleEffect(layerVar, scribble);
                    else
                        throw new NotSupportedException(drawing.GetType().FullName);
                }
            }

            if (textDrawings.Any())
            {
                var layerVar = _context.GetNextAutoVariable();
                _builder.AppendLine($"var {layerVar} = {compositionRef}.layers.addText()");

                if (layer.InPoint != null)
                    _builder.AppendLine($"{layerVar}.inPoint = {layer.InPoint};");

                if (layer.OutPoint != null)
                    _builder.AppendLine($"{layerVar}.outPoint = {layer.OutPoint};");

                _builder.AppendLine($"{layerVar}.parent = {nullLayerVar};");

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
            var baseGroupVar = _context.GetNextAutoVariable();
            var vectorsGroupVar = _context.GetNextAutoVariable();
            var vectorGroupVar = _context.GetNextAutoVariable();
            var strokeVar = _context.GetNextAutoVariable();

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

            _builder.AppendLine(scriptText);
        }

        private void VisitText(string layerVar, AdobeTextControl text)
        {
            var adobeIndex = 1;
            foreach (var value in text.Values)
            {
                _builder.AppendLine($"{layerVar}.property('ADBE Text Properties').property('ADBE Text Document').setValueAtTime({value.Time}, '{value.Value}');");
                _builder.AppendLine($"{layerVar}.property('ADBE Text Properties').property('ADBE Text Document').setInterpolationTypeAtKey({adobeIndex}, KeyframeInterpolationType.HOLD, KeyframeInterpolationType.HOLD);");
            }
        }

        private void VisitColorControl(string layerVar, AdobeSharedColorControl colorControl)
        {
            var colorControlVar = _context.GetNextAutoVariable();

            _builder.AppendLine(@$"var {colorControlVar} = {layerVar}.effect.addProperty('ADBE Color Control');
{colorControlVar}.name = '{colorControl.ControlName}';");
        }

        private void VisitSlider(string compositionRef, AdobeSliderControl slider)
        {
            var nullLayerVar = _context.GetNextAutoVariable();

            _builder.AppendLine(@$"var {nullLayerVar} = {compositionRef}.layers.addNull();
{nullLayerVar}.effect.addProperty('ADBE Slider Control')('Slider');");

            var adobeIndex = 1;
            foreach (var value in slider.Values)
            {
                _builder.AppendLine($"{nullLayerVar}.effect('Slider Control').property('Slider').setValueAtTime({value.Time}, {value.Value});");
                _builder.AppendLine($"{nullLayerVar}.effect('Slider Control').property('Slider').setInterpolationTypeAtKey({adobeIndex}, KeyframeInterpolationType.HOLD, KeyframeInterpolationType.HOLD);");
            }

            if (!string.IsNullOrEmpty(slider.Name))
                _builder.AppendLine($"{nullLayerVar}.name = { slider.Name};");
        }

        private void VisitMask(string layerVar, AdobeMaskComponent mask)
        {
            var maskVar = _context.GetNextAutoVariable();
            var maskShapeVar = _context.GetNextAutoVariable();

            var scriptText = $@"var {maskVar} = {layerVar}.Masks.addProperty('Mask');
{maskVar}.inverted = {mask.IsInverted.ToString().ToLower()}
var {maskShapeVar} = {maskVar}.property('maskShape');
{CreateSetVerticesCode(mask.PathComponent.Points, maskShapeVar, mask.PathComponent.IsClosed, true)}";

            if (!string.IsNullOrEmpty(mask.MaskName))
                scriptText = string.Join(Environment.NewLine, scriptText, $"{maskVar}.name = '{mask.MaskName}';");

            _builder.AppendLine(scriptText);
        }

        private void VisitScribbleEffect(string layerVar, AdobeScribbleEffect scribbleEffect)
        {
            var scribbleVar = _context.GetNextAutoVariable();

            var scriptText = $@"var {scribbleVar} = {layerVar}.Effects.addProperty('ADBE Scribble Fill');
{scribbleVar}.Mask = '{scribbleEffect.MaskName}';";

            if (scribbleEffect.ColorValue != null)
            {
                scriptText = string.Join(Environment.NewLine, scriptText, $"{scribbleVar}.Color{scribbleEffect.ColorValue.GetScriptText()};");
            }

            if (scribbleEffect.Start != null)
            {
                string additionalText;
                if (!scribbleEffect.Start.IsAnimated)
                    additionalText = $"{scribbleVar}.property('Start').setValue({scribbleEffect.Start.GetValues().Single().Value});";
                else
                {
                    additionalText = string.Join(Environment.NewLine, scribbleEffect.Start.GetValues()
                        .Select(x => $"{scribbleVar}.property('Start').setValueAtTime({x.Time.Time}, {x.Value});"));
                }

                scriptText = String.Join(Environment.NewLine, scriptText, additionalText);
            }

            if (scribbleEffect.End != null)
            {
                string additionalText;
                if (!scribbleEffect.End.IsAnimated)
                    additionalText = $"{scribbleVar}.property('End').setValue({scribbleEffect.End.GetValues().Single().Value});";
                else
                {
                    additionalText = string.Join(Environment.NewLine, scribbleEffect.End.GetValues()
                        .Select(x => $"{scribbleVar}.property('End').setValueAtTime({x.Time.Time}, {x.Value});"));
                }

                scriptText = String.Join(Environment.NewLine, scriptText, additionalText);
            }

            _builder.AppendLine(scriptText);
        }

        private void VisitText(string compositionRef, AdobeTextComponent text)
        {
            var lines = new List<string>();

            //https://ae-scripting.docsforadobe.dev/other/textdocument.html?highlight=TextDocument#textdocument
            var textDocVar = _context.GetNextAutoVariable();
            lines.Add($"var {textDocVar} = new TextDocument('{text.TextValue}');");

            //Make sure to add the text document to the layer before setting properties on the layer
            //otherwise a runtime exception will be thrown by adobe
            //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addtext
            var layerVar = _context.GetNextAutoVariable();
            lines.Add($"var {layerVar} = {compositionRef}.layers.addText('{text.TextValue}');");
            lines.Add($"{layerVar}.position.setValue([{text.Bounds.Left + text.Bounds.Width}, {text.Bounds.Top + text.Bounds.Height - GetFontHeightCorrection(text.Bounds.Height)}]);");

            //The source text needs to be saved and then reset or else it doesn't work, which is weird. The idea was taken from:
            //https://community.adobe.com/t5/after-effects-discussions/unable-to-execute-script-at-line-17-unable-to-set-value-as-it-is-not-associated-with-a-layer/td-p/11782185
            var sourceTextVar = _context.GetNextAutoVariable();
            lines.Add(@$"var {sourceTextVar} = {layerVar}.text.sourceText;
var {textDocVar} = {sourceTextVar}.value;
{textDocVar}.font = '{text.TextSettings.FontName}';
{textDocVar}.fontSize = {text.TextSettings.FontSizeInPixels};
{textDocVar}.justification = ParagraphJustification.RIGHT_JUSTIFY;
{sourceTextVar}.setValue({textDocVar});");

            _builder.Append(string.Join(Environment.NewLine, lines.ToArray()));
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
            var shapeVar = _context.GetNextAutoVariable();

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
    }
}
