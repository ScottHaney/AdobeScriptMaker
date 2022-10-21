using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering.Drawing
{
    public class AnimatedDrawing
    {
        public readonly PathDrawing Drawing;
        public readonly float DrawingTime;

        public AnimatedDrawing(PathDrawing drawing, float drawingTime)
        {
            Drawing = drawing;
            DrawingTime = drawingTime;
        }
    }
}
