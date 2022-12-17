using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using MatrixLayout.InputDescriptions;
using RenderingDescriptions.What;

namespace AdobeScriptMaker.Core
{
    public class MatrixScriptCreator
    {
        public string CreateScript(params ILayoutResults[] layoutResults)
        {
            var context = new ScriptContext();

            var compositionItem = "app.project.activeItem";

            var results = new StringBuilder();

            foreach (var layoutResult in layoutResults)
            {
                var nullLayerVar = context.GetNextAutoVariable();
                results.AppendLine($"var {nullLayerVar} = {compositionItem}.layers.addNull();");

                foreach (var result in layoutResult.GetResults())
                {
                    if (result is MatrixEntryLayoutResult entryResult)
                    {
                        results.AppendLine(CreateTextLayer(context, nullLayerVar, compositionItem, entryResult.Text, entryResult.Bounds, entryResult.TextSettings));
                    }
                    else if (result is TextLayoutResult textResult)
                    {
                        results.AppendLine(CreateTextLayer(context, nullLayerVar, compositionItem, textResult.Text, textResult.Bounds, textResult.TextSettings));
                    }
                    else if (result is MatrixBracketsLayoutResult bracketsResult)
                    {
                        results.AppendLine(CreatePathLayer(context, nullLayerVar, compositionItem, bracketsResult.BracketsSettings, bracketsResult.GetLeftBracketPathPoints(), bracketsResult.GetRightBracketPathPoints()));
                    }
                }
            }

            return results.ToString();
        }

        private string CreateTextLayer(ScriptContext context, string nullLayerVar, string adobeCompositionItem, string value, RectangleF bounds, TextSettings textSettings)
        {
            var lines = new List<string>();

            //https://ae-scripting.docsforadobe.dev/other/textdocument.html?highlight=TextDocument#textdocument
            var textDocVar = context.GetNextAutoVariable();
            lines.Add($"var {textDocVar} = new TextDocument('{value}');");

            //Make sure to add the text document to the layer before setting properties on the layer
            //otherwise a runtime exception will be thrown by adobe
            //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addtext
            var layerVar = context.GetNextAutoVariable();
            lines.Add($"var {layerVar} = {adobeCompositionItem}.layers.addText('{value}');");
            lines.Add($"{layerVar}.position.setValue([{bounds.Left + bounds.Width}, {bounds.Top + bounds.Height - GetFontHeightCorrection(bounds.Height)}]);");

            //The source text needs to be saved and then reset or else it doesn't work, which is weird. The idea was taken from:
            //https://community.adobe.com/t5/after-effects-discussions/unable-to-execute-script-at-line-17-unable-to-set-value-as-it-is-not-associated-with-a-layer/td-p/11782185
            var sourceTextVar = context.GetNextAutoVariable();
            lines.Add(@$"var {sourceTextVar} = {layerVar}.text.sourceText;
var {textDocVar} = {sourceTextVar}.value;
{textDocVar}.font = '{textSettings.FontName}';
{textDocVar}.fontSize = {textSettings.FontSizeInPixels};
{textDocVar}.justification = ParagraphJustification.RIGHT_JUSTIFY;
{sourceTextVar}.setValue({textDocVar});
{layerVar}.parent = {nullLayerVar};");

            return string.Join(Environment.NewLine, lines.ToArray());
        }

        private float GetFontHeightCorrection(float height)
        {
            return height * 0.15f;
        }

        private string CreatePathLayer(ScriptContext context, string nullLayerVar, string adobeCompositionItem, MatrixBracketsDescription bracketsSettings, List<PointF> leftBracketPoints, List<PointF> rightBracketPoints)
        {
            var lines = new List<string>();

            //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addshape
            //For a tutorial on how to add paths to shape layers: https://www.youtube.com/watch?v=zGbd-tEyryg
            var shapeLayerVar = context.GetNextAutoVariable();

            lines.Add(@$"var {shapeLayerVar} = {adobeCompositionItem}.layers.addShape();
{CreateBracketsScript(context, shapeLayerVar, bracketsSettings, leftBracketPoints)}
{CreateBracketsScript(context, shapeLayerVar, bracketsSettings, rightBracketPoints)}
{shapeLayerVar}.parent = {nullLayerVar};");

            return string.Join(Environment.NewLine, lines.ToArray());
        }

        private string CreateBracketsScript(ScriptContext context,
            string shapeLayerVar,
            MatrixBracketsDescription bracketsSettings,
            List<PointF> bracketPoints)
        {
            var bracketsPathGroupVar = context.GetNextAutoVariable();
            var bracketPathVar = context.GetNextAutoVariable();
            var bracketShapeVar = context.GetNextAutoVariable();
            var strokeGroupVar = context.GetNextAutoVariable();

            return $@"var {bracketsPathGroupVar} = {shapeLayerVar}.property('Contents').addProperty('ADBE Vector Group').addProperty('ADBE Vectors Group');
var {bracketPathVar} = {bracketsPathGroupVar}.addProperty('ADBE Vector Shape - Group')
var {bracketShapeVar} = new Shape();
{bracketShapeVar}.vertices = { ConvertPointsToJavascriptArg(bracketPoints)};
{bracketShapeVar}.closed = true;
{bracketPathVar}.property('Path').setValue({ bracketShapeVar});
var {strokeGroupVar} = {bracketsPathGroupVar}.addProperty('ADBE Vector Graphic - Stroke');
{strokeGroupVar}.property('ADBE Vector Stroke Width').setValue('{bracketsSettings.Thickness}');
{strokeGroupVar}.property('ADBE Vector Stroke Color').setValue([0, 0, 0]);
var w = {shapeLayerVar}.property('Transform').property('Position').setValue([0, 0]);";
        }

        private string ConvertPointsToJavascriptArg(List<PointF> points)
        {
            var pointArgs = string.Join(",", points.Select(x => $"[{x.X},{x.Y}]"));
            return $"[{pointArgs}]";
        }
    }

    public class ScriptContext
    {
        private int _numAutoVariables = 0;

        public string GetNextAutoVariable()
        {
            _numAutoVariables++;
            return $"v{_numAutoVariables}";
        }
    }
}
