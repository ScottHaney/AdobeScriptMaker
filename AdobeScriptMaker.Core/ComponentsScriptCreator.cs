using AdobeScriptMaker.Core.Components;
using AdobeScriptMaker.Core.Components.Layers;
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
            foreach (var layer in composition.Layers)
                VisitLayer("app.project.activeItem", layer);
        }

        private void VisitLayer(string compositionRef, AdobeShapeLayer layer)
        {
            var layerVar = _context.GetNextAutoVariable();
            _builder.AppendLine($"var {layerVar} = {compositionRef}.layers.addShape()");

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
{shapeVar}.vertices = { ConvertPointsToJavascriptArg(path.Points)};
{shapeVar}.closed = {path.IsClosed.ToString().ToLower()};
{vectorGroupVar}.property('Path').setValue({shapeVar});
var {strokeVar} = {vectorsGroupVar}.addProperty('ADBE Vector Graphic - Stroke');
{strokeVar}.property('ADBE Vector Stroke Width').setValue('{path.Thickness}');
{strokeVar}.property('ADBE Vector Stroke Color').setValue([0, 0, 0]);
{layerVar}.property('Transform').property('Position').setValue([0, 0]);

var {transformGroupVar} = {baseGroupVar}.property('ADBE Vector Transform Group');
var {scaleVar} = {transformGroupVar}.property('ADBE Vector Scale');
";

            _builder.AppendLine(scriptText);
        }

        private string ConvertPointsToJavascriptArg(IEnumerable<PointF> points)
        {
            var pointArgs = string.Join(",", points.Select(x => $"[{x.X},{x.Y}]"));
            return $"[{pointArgs}]";
        }
    }
}
