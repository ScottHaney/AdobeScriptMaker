using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DirectRendering.Drawing
{
    public class PathDrawing
    {
        public readonly Point[] Points;

        public PathDrawing(params Point[] points)
        {
            Points = points ?? Array.Empty<Point>();
        }
    }
}
