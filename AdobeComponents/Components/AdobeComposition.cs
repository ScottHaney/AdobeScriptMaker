using AdobeComponents.Components.Layers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Components
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
