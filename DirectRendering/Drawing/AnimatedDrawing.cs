using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering.Drawing
{
    public class AnimatedDrawing
    {
        public readonly LineDrawing Drawing;
        public readonly float DrawingTime;

        public AnimatedDrawing(LineDrawing drawing, float drawingTime)
        {
            Drawing = drawing;
            DrawingTime = drawingTime;
        }
    }
}
