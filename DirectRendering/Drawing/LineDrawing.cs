using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DirectRendering.Drawing
{
    public class LineDrawing
    {
        public readonly Point Start;
        public readonly Point End;

        public LineDrawing(Point start, Point end)
        {
            Start = start;
            End = end;
        }
    }
}
