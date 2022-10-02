using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public interface ILayoutResults
    {
        IEnumerable<ILayoutResult> GetResults();
        RectangleF BoundingBox { get; }
        void ShiftDown(float shift);
    }
}
