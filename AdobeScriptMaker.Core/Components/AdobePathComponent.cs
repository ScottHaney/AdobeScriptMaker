using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DirectRendering.Drawing.Animation;

namespace AdobeScriptMaker.Core.Components
{
    public class AdobePathComponent
    {
        public readonly IAnimatedValue<PointF[]> Points;
        public float Thickness { get; set; } = 2;
        public bool IsClosed { get; set; }
        public bool HasLockedScale { get; set; } = true;

        public AdobePathComponent(IAnimatedValue<PointF[]> points)
        {
            Points = points;
        }
    }
}
