using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Text;

namespace AdobeScriptMaker.Core
{
    public class MatrixScriptCreator
    {
        public string CreateScript(ILayoutResults layoutResults)
        {
            foreach (var result in layoutResults.GetResults())
            {
                if (result is TextLayoutResult textLayoutResult)
                {

                }
            }

            throw new NotImplementedException();
        }

        private string CreateTextLayer(string adobeCompositionItem, string value, string fontName, double fontSize)
        {
            var lines = new StringBuilder();

            //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addtext
            lines.AppendLine($"var layer = {adobeCompositionItem}.layers.addText(new TextDocument('{value}'));");

            return lines.ToString();
        }
    }
}
