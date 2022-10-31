using AdobeScriptMaker.Core.Components;
using AdobeScriptMaker.Core.Components.Layers;
using DirectRendering.Drawing.Animation;
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

        private void VisitLayer(string compositionRef, AdobeShapeLayer layer, string nullLayerVar)
        {
            var layerVar = _context.GetNextAutoVariable();

            _builder.AppendLine($"var {layerVar} = {compositionRef}.layers.addShape()");
            _builder.AppendLine($"{layerVar}.parent = {nullLayerVar};");

            foreach (var drawing in layer.Drawings)
                VisitPath(layerVar, drawing);
        }

        private void VisitPath(string layerVar, AdobePathComponent path)
        {
            var baseGroupVar = _context.GetNextAutoVariable();
            var vectorsGroupVar = _context.GetNextAutoVariable();
            var vectorGroupVar = _context.GetNextAutoVariable();
            var shapeVar = _context.GetNextAutoVariable();
            var strokeVar = _context.GetNextAutoVariable();

            var transformGroupVar = _context.GetNextAutoVariable();
            var scaleVar = _context.GetNextAutoVariable();

            var scriptText = $@"var {baseGroupVar} = {layerVar}.property('Contents').addProperty('ADBE Vector Group');
var {vectorsGroupVar} = {baseGroupVar}.addProperty('ADBE Vectors Group');
var {vectorGroupVar} = {vectorsGroupVar}.addProperty('ADBE Vector Shape - Group')
var {shapeVar} = new Shape();
{CreateSetVerticesCode(path.Points, shapeVar)}
{shapeVar}.closed = {path.IsClosed.ToString().ToLower()};
{vectorGroupVar}.property('Path').setValue({shapeVar});
var {strokeVar} = {vectorsGroupVar}.addProperty('ADBE Vector Graphic - Stroke');
{strokeVar}.property('ADBE Vector Stroke Width').setValue('{path.Thickness}');
{strokeVar}.property('ADBE Vector Stroke Color').setValue([0, 0, 0]);
{layerVar}.property('Transform').property('Position').setValue([0, 0]);

var {transformGroupVar} = {baseGroupVar}.property('ADBE Vector Transform Group');
var {scaleVar} = {transformGroupVar}.property('ADBE Vector Scale');";

            _builder.AppendLine(scriptText);
        }

        private string CreateSetVerticesCode(IAnimatedValue<PointF[]> points,
            string shapeVar)
        {
            if (!points.IsAnimated)
                return $"{shapeVar}.vertices = { ConvertPointsToJavascriptArg(points.GetValues().Single().Value)};";
            else
            {
                return String.Join(Environment.NewLine,
                    points.GetValues()
                        .Select(x => $"{shapeVar}.vertices.setValueAtTime({x.Time.Time.Value}, { ConvertPointsToJavascriptArg(points.GetValues().Single().Value)});"));
            }
        }

        private string ConvertPointsToJavascriptArg(IEnumerable<PointF> points)
        {
            var pointArgs = string.Join(",", points.Select(x => $"[{x.X},{x.Y}]"));
            return $"[{pointArgs}]";
        }
    }
}
