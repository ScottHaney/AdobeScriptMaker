using AdobeComponents.Animation;
using AdobeComponents.CommonValues;
using AdobeComponents.Components;
using AdobeComponents.Components.Layers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Effects
{
    public class AdobeScribbleEffect
    {
        public readonly string MaskName;

        public IAdobeColorValue ColorValue { get; set; }

        public IAnimatedValue<double> Start { get; set; }

        public IAnimatedValue<double> End { get; set; }

        public IAdobeSliderValue WigglesPerSecond { get; set; } = new AdobeSliderValue(10);

        public AdobeScribbleEffect(string maskName)
        {
            MaskName = maskName;
        }
    }
}
