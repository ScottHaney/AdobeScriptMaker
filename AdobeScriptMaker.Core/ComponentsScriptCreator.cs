﻿using AdobeComponents.Animation;
using AdobeComponents.Components;
using AdobeComponents.Components.Layers;
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
            _builder.AppendLine($"var {nullLayerVar} = {compositionRef}.layers.addNull();");

            foreach (var layer in composition.Layers)
                VisitLayer(compositionRef, layer, nullLayerVar);
        }

        private void VisitLayer(string compositionRef, AdobeLayer layer, string nullLayerVar)
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
                    if (drawing is AdobePathComponent path)
                        VisitPath(layerVar, path);
                    else if (drawing is AdobeSliderControl slider)
                        VisitSlider(compositionRef, slider);
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

            var transformGroupVar = _context.GetNextAutoVariable();
            var scaleVar = _context.GetNextAutoVariable();

            var scriptText = $@"var {baseGroupVar} = {layerVar}.property('Contents').addProperty('ADBE Vector Group');
var {vectorsGroupVar} = {baseGroupVar}.addProperty('ADBE Vectors Group');
var {vectorGroupVar} = {vectorsGroupVar}.addProperty('ADBE Vector Shape - Group')
{CreateSetVerticesCode(path.Points, vectorGroupVar, path.IsClosed)}
var {strokeVar} = {vectorsGroupVar}.addProperty('ADBE Vector Graphic - Stroke');
{strokeVar}.property('ADBE Vector Stroke Width').setValue('{path.Thickness}');
{strokeVar}.property('ADBE Vector Stroke Color').setValue([0, 0, 0]);
{layerVar}.property('Transform').property('Position').setValue([0, 0]);

var {transformGroupVar} = {baseGroupVar}.property('ADBE Vector Transform Group');
var {scaleVar} = {transformGroupVar}.property('ADBE Vector Scale');";

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

        private string CreateSetVerticesCode(IAnimatedValue<PointF[]> points,
            string vectorGroupVar,
            bool isClosed)
        {
            if (!points.IsAnimated)
                return CreateShape(points.GetValues().Single().Value, isClosed, vectorGroupVar, null);
            else
            {
                return String.Join(Environment.NewLine,
                    points.GetValues()
                        .Select(x => CreateShape(x.Value, isClosed, vectorGroupVar, x.Time.Time)));
            }
        }

        private string CreateShape(IEnumerable<PointF> points,
            bool isClosed,
            string vectorGroupVar,
            double? time)
        {
            var shapeVar = _context.GetNextAutoVariable();

            var result = $@"var {shapeVar} = new Shape();
{shapeVar}.vertices = {ConvertPointsToJavascriptArg(points)};
{shapeVar}.closed = {isClosed.ToString().ToLower()};";

            if (time == null)
                result = string.Join(Environment.NewLine, result, $"{vectorGroupVar}.property('Path').setValue({shapeVar});");
            else
                result = string.Join(Environment.NewLine, result, $"{vectorGroupVar}.property('Path').setValueAtTime({time}, {shapeVar});");

            return result;

        }

        private string ConvertPointsToJavascriptArg(IEnumerable<PointF> points)
        {
            var pointArgs = string.Join(",", points.Select(x => $"[{x.X},{x.Y}]"));
            return $"[{pointArgs}]";
        }
    }
}
