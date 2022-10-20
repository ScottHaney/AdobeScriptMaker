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

            var result = new StringBuilder();
            foreach (var composition in compositions)
            {
                var compositionResult = CreateCompositionItem(composition);
                result.AppendLine(compositionResult.CreationCode);


            }

            return result.ToString();
        }

        private CompositionItemResult CreateCompositionItem(AdobeComposition composition)
        {
            if (composition.IsDefaultComp)
                return new CompositionItemResult("", "app.project.activeItem");
            else
                throw new NotImplementedException();
        }

        private class CompositionItemResult
        {
            public readonly string CreationCode;
            public readonly string CompositionVariable;

            public CompositionItemResult(string creationCode, string compositionVariable)
            {
                CreationCode = creationCode;
                CompositionVariable = compositionVariable;
            }
        }
    }
}
