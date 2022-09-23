using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Text;

namespace AdobeScriptMaker.Core
{
    public class MatrixScriptCreator
    {
        public string CreateScript(ILayoutResults layoutResults)
        {
            var compositionItem = "app.project.activeItem";

            var results = new StringBuilder();
            foreach (var result in layoutResults.GetResults())
            {
                if (result is MatrixEntryLayoutResult entryResult)
                {
                    results.AppendLine(CreateTextLayer(compositionItem, entryResult.Text, "Arial", 12));
                }
            }

            return results.ToString();
        }

        private string CreateTextLayer(string adobeCompositionItem, string value, string fontName, double fontSize)
        {
            var lines = new StringBuilder();

            //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addtext
            lines.Append($"var layer = {adobeCompositionItem}.layers.addText(new TextDocument('{value}'));");

            return lines.ToString();
        }
    }
}
