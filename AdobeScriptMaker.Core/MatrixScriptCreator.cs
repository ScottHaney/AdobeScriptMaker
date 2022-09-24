using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace AdobeScriptMaker.Core
{
    public class MatrixScriptCreator
    {
        public string CreateScript(ILayoutResults layoutResults)
        {
            var context = new ScriptContext();

            var compositionItem = "app.project.activeItem";

            var results = new StringBuilder();
            foreach (var result in layoutResults.GetResults())
            {
                if (result is MatrixEntryLayoutResult entryResult)
                {
                    results.AppendLine(CreateTextLayer(context, compositionItem, entryResult.Text, entryResult.Bounds, entryResult.TextSettings));
                }
                else if (result is MatrixBracketsLayoutResult bracketsResult)
                {
                    results.AppendLine(CreatePathLayer(context, compositionItem));
                }
            }

            return results.ToString();
        }

        private string CreateTextLayer(ScriptContext context, string adobeCompositionItem, string value, RectangleF bounds, TextSettings textSettings)
        {
            var lines = new List<string>();

            //https://ae-scripting.docsforadobe.dev/other/textdocument.html?highlight=TextDocument#textdocument
            var textDocVar = context.GetNextAutoVariable();
            lines.Add($"var {textDocVar} = new TextDocument('{value}');");

            //Make sure to add the text document to the layer before setting properties on the layer
            //otherwise a runtime exception will be thrown by adobe
            //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addtext
            var layerVar = context.GetNextAutoVariable();
            lines.Add($"var {layerVar} = {adobeCompositionItem}.layers.addText({value});");
            lines.Add($"{layerVar}.position.setValue([{ bounds.Left}, {bounds.Top}]);");

            //The source text needs to be saved and then reset or else it doesn't work, which is weird. The idea was taken from:
            //https://community.adobe.com/t5/after-effects-discussions/unable-to-execute-script-at-line-17-unable-to-set-value-as-it-is-not-associated-with-a-layer/td-p/11782185
            var sourceTextVar = context.GetNextAutoVariable();
            lines.Add(@$"var {sourceTextVar} = {layerVar}.text.sourceText;
var {textDocVar} = {sourceTextVar}.value;
{textDocVar}.font = '{textSettings.FontName}';
{textDocVar}.fontSize = {textSettings.FontSize};
{textDocVar}.justification = ParagraphJustification.RIGHT_JUSTIFY;
{sourceTextVar}.setValue({textDocVar});");

            return string.Join(Environment.NewLine, lines.ToArray());
        }

        private string CreatePathLayer(ScriptContext context, string adobeCompositionItem)
        {
            var lines = new List<string>();

            //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addshape
            //For a tutorial on how to add paths to shape layers: https://www.youtube.com/watch?v=zGbd-tEyryg
            var shapeLayerVar = context.GetNextAutoVariable();
            var leftBracketPathVar = context.GetNextAutoVariable();
            var leftBracketShapeVar = context.GetNextAutoVariable();

            lines.Add(@$"var {shapeLayerVar} = {adobeCompositionItem}.layers.addShape();
var {leftBracketPathVar} = {shapeLayerVar}.property('Contents').addProperty('ADBE Vector Group').addProperty('ADBE Vectors Group').addProperty('ADBE Vector Shape - Group');
var {leftBracketShapeVar} = new Shape();
{leftBracketShapeVar}.vertices = [[0,0], [100,100], [100,0], [0, 100]];
{leftBracketShapeVar}.closed = true;
{leftBracketPathVar}.property('Path').setValue({leftBracketShapeVar});");

            return string.Join(Environment.NewLine, lines.ToArray());
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
