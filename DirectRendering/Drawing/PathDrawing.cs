using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DirectRendering.Drawing
{
    public class PathDrawing : PrimitiveDrawing
    {
        public readonly Point[] Points;

        public float Thickness { get; set; } = 2;
        public bool IsClosed { get; set; }
        public bool HasLockedScale { get; set; } = true;

        public PathDrawing(params Point[] points)
        {
            Points = points ?? Array.Empty<Point>();
        }
    }
}
