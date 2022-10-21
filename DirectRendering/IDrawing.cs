using DirectRendering.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering
{
    public interface IDrawing
    {
        IEnumerable<IDrawing> GetDrawings();
    }
}
