using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public interface ILayoutResult
    {
        RectangleF Bounds { get; }
    }
}
