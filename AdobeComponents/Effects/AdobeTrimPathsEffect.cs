using AdobeComponents.Animation;
using AdobeComponents.CommonValues;
using AdobeComponents.Components;
using AdobeComponents.Components.Layers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Effects
{
    public class AdobeTrimPathsEffect : IAdobeLayerComponent
    {
        public IAnimatedValue<double> Start { get; set; }

        public IAnimatedValue<double> End { get; set; }

        public AdobeTrimPathsEffect()
        {
        }
    }
}
