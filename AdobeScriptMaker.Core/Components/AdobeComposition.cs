using AdobeScriptMaker.Core.Components.Layers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.Core.Components
{
    public class AdobeComposition
    {
        public readonly AdobeLayer[] Layers;

        public AdobeComposition(params AdobeLayer[] layers)
        {
            Layers = layers ?? Array.Empty<AdobeLayer>();
        }
    }
}
