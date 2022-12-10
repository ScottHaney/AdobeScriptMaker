using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Components
{
    public class AdobeScript
    {
        public readonly AdobeComposition[] Compositions;

        public AdobeScript(params AdobeComposition[] compositions)
        {
            Compositions = compositions ?? Array.Empty<AdobeComposition>();
        }
    }
}
