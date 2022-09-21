using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class TextLayoutResult : ILayoutResult
    {
        public RectangleF Bounds { get; private set; }

        public TextLayoutResult(RectangleF bounds)
        {
            Bounds = bounds;
        }
    }
}
