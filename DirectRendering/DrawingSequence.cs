using DirectRendering.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering
{
    public class DrawingSequence
    {
        public readonly IDrawing[] Drawings;

        public DrawingSequence(params IDrawing[] drawings)
        {
            Drawings = (drawings ?? Array.Empty<IDrawing>());
        }
    }
}
