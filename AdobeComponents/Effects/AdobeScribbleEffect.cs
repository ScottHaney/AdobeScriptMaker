using AdobeComponents.Animation;
using AdobeComponents.CommonValues;
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

        public IAdobeColorValue ColorValue { get; set; }

        public IAnimatedValue<double> End { get; set; }

        public AdobeScribbleEffect(string maskName)
        {
            MaskName = maskName;
        }
    }
}
