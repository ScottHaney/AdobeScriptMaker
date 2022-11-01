using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DirectRendering.Drawing.Animation;

namespace DirectRendering.Drawing
{
    public class PathDrawing : PrimitiveDrawing
    {
        public readonly IAnimatedValue<PointF[]> Points;

        public float Thickness { get; set; } = 2;
        public bool IsClosed { get; set; }
        public bool HasLockedScale { get; set; } = true;

        public PathDrawing(params PointF[] points)
        {
            Points = new StaticValue<PointF[]>(points ?? Array.Empty<PointF>());
        }

        public PathDrawing(IAnimatedValue<PointF[]> points)
        {
            Points = points;
        }
    }
}
