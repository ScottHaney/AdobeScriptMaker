using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.Core.Components.Layers
{
    public class AdobeLayer
    {
        public double? InPoint { get; set; }
        public double? OutPoint { get; set; }

        public readonly IAdobeLayerComponent[] Drawings;

        public AdobeLayer(params IAdobeLayerComponent[] drawings)
        {
            Drawings = drawings ?? Array.Empty<IAdobeLayerComponent>();
        }
    }
}
