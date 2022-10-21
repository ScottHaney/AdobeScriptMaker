using DirectRendering.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering
{
    public class DrawingSequence
    {
        public readonly PathDrawing[] Drawings;

        public DrawingSequence(PathDrawing[] drawings)
        {
            Drawings = drawings ?? Array.Empty<PathDrawing>();
        }
    }
}
