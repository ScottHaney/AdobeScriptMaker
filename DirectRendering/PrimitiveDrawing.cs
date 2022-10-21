using DirectRendering.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering
{
    public class PrimitiveDrawing : IDrawing
    {
        public IEnumerable<IDrawing> GetDrawings()
        {
            yield return this;
        }
    }
}
