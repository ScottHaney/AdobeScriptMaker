using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.Core.Components.Layers
{
    public class AdobeShapeLayer : AdobeLayer
    {
        public readonly AdobePathComponent[] Drawings;

        public AdobeShapeLayer(params AdobePathComponent[] drawings)
        {
            Drawings = drawings ?? Array.Empty<AdobePathComponent>();
        }
    }
}
