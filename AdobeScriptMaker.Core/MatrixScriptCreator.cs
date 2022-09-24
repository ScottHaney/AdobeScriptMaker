using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Text;

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
                    results.AppendLine(CreateTextLayer(context, compositionItem, entryResult.Text, "Arial", 12));
                }
            }

            return results.ToString();
        }

        private string CreateTextLayer(ScriptContext context, string adobeCompositionItem, string value, string fontName, double fontSize)
        {
            var lines = new StringBuilder();

            //https://ae-scripting.docsforadobe.dev/other/textdocument.html?highlight=TextDocument#textdocument
            var textDocVar = context.GetNextAutoVariable();
            lines.AppendLine(@$"var {textDocVar} = new TextDocument('{value}');
{textDocVar}.font = 'Arial';
{textDocVar}.fontSize = 12;
{textDocVar}.justification = ParagraphJustification.RIGHT_JUSTIFY;");

            //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addtext
            lines.Append($"{adobeCompositionItem}.layers.addText({textDocVar});");

            return lines.ToString();
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
