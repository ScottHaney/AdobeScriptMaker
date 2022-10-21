using AdobeScriptMaker.Core.Components.Layers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.Core.Components
{
    public class AdobeComposition
    {
        public readonly AdobeShapeLayer[] Layers;

        public AdobeComposition(params AdobeShapeLayer[] layers)
        {
            Layers = layers ?? Array.Empty<AdobeShapeLayer>();
        }
    }
}
