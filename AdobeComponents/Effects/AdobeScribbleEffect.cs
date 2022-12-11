using AdobeComponents.Components;
using AdobeComponents.Components.Layers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Effects
{
    public class AdobeScribbleEffect : IAdobeLayerComponent
    {
        public readonly string MaskName;

        public AdobeScribbleEffect(string maskName)
        {
            MaskName = maskName;
        }
    }
}
