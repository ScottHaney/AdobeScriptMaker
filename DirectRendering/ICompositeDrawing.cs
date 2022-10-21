using DirectRendering.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectRendering
{
    public class CompositeDrawing : IDrawing
    {
        private readonly IDrawing[] _children;

        public CompositeDrawing(params IDrawing[] children)
        {
            _children = children ?? Array.Empty<IDrawing>();
        }

        public IEnumerable<IDrawing> GetDrawings()
        {
            return _children.SelectMany(x => x.GetDrawings());
        }
    }
}
