using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.Core.Components.Layers
{
    public class AdobeShapeLayer : AdobeLayer
    {
        public readonly IAdobeLayerComponent[] Drawings;

        public AdobeShapeLayer(params IAdobeLayerComponent[] drawings)
        {
            Drawings = drawings ?? Array.Empty<IAdobeLayerComponent>();
        }
    }
}
