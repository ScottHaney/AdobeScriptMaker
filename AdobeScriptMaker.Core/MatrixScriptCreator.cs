﻿using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Collections.Generic;
using System.Drawing;
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
                    results.AppendLine(CreateTextLayer(context, compositionItem, entryResult.Text, entryResult.Bounds, entryResult.TextSettings));
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

            var sourceTextVar = context.GetNextAutoVariable();
            lines.Add($"var {sourceTextVar} = {layerVar}.text.sourceText;");
            lines.Add($"var {textDocVar} = {sourceTextVar}.value;");
            lines.Add($@"{textDocVar}.font = '{textSettings.FontName}';
{textDocVar}.fontSize = {textSettings.FontSize};
{textDocVar}.justification = ParagraphJustification.RIGHT_JUSTIFY;");
            lines.Add($"{sourceTextVar}.setValue({textDocVar});");

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
