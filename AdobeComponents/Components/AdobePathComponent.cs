using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AdobeComponents.Animation;
using AdobeComponents.CommonValues;
using AdobeComponents.Effects;

namespace AdobeComponents.Components
{
    public class AdobePathComponent : IAdobeSupportsMaskComponent, IAdobeSupportsScribbleEffect
    {
        public readonly IAnimatedValue<PointF[]> Points;
        public float Thickness { get; set; } = 2;
        public bool IsClosed { get; set; }
        public bool HasLockedScale { get; set; } = true;

        public AdobeTrimPathsEffect TrimPaths { get; set; }

        public IAdobeColorValue ColorValue { get; set; }

        public AdobeMaskComponent Mask { get; set; }

        public AdobeScribbleEffect ScribbleEffect { get; set; }

        public AdobePathComponent(IAnimatedValue<PointF[]> points)
        {
            Points = points;
        }
    }
}
