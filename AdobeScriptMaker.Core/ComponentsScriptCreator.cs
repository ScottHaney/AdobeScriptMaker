using AdobeScriptMaker.Core.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.Core
{
    public class ComponentsScriptCreator
    {
        public string CreateScript(params AdobeComposition[] compositions)
        {
            var context = new ScriptContext();
            var compositionItem = "app.project.activeItem";

            var result = new StringBuilder();
            foreach (var composition in compositions)
                result.AppendLine(CreateComposition(composition));

            return result.ToString();
        }

        private string CreateComposition(AdobeComposition composition)
        {
            throw new NotImplementedException();
        }
    }
}
