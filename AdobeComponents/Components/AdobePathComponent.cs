using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AdobeComponents.Animation;
using AdobeComponents.CommonValues;

namespace AdobeComponents.Components
{
    public class AdobePathComponent : IAdobeLayerComponent
    {
        public readonly IAnimatedValue<PointF[]> Points;
        public float Thickness { get; set; } = 2;
        public bool IsClosed { get; set; }
        public bool HasLockedScale { get; set; } = true;

        public IAdobeColorValue ColorValue { get; set; }

        public AdobePathComponent(IAnimatedValue<PointF[]> points)
        {
            Points = points;
        }
    }
}
