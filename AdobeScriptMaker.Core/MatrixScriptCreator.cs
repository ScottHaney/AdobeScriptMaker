using MatrixLayout.ExpressionLayout.LayoutResults;
using System;

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

        private string CreateTextLayer(string source, string value, string fontName, double fontSize)
        {
            //https://ae-scripting.docsforadobe.dev/layers/layercollection.html#layercollection-addtext
            var layer = $"var layer = {source}.layers.addText({value});";

            //https://www.youtube.com/watch?v=6P76aFYmOR8&t=11s
            var setText = $@"var textDocument = new TextDocument('{value}');
textDocument.text = '{value}';
textDocument.font = '{fontName}';
layer.property('Source Text').setValue(textDocument);";

            return string.Join(Environment.NewLine, layer, setText);
        }
    }
}
