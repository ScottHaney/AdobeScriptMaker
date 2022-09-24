using MatrixLayout.InputDescriptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class MatrixBracketsLayoutResult : ILayoutResult
    {
        public RectangleF Bounds { get; private set; }
        public readonly MatrixBracketsDescription BracketsSettings;

        public MatrixBracketsLayoutResult(RectangleF outerBounds,
            MatrixBracketsDescription bracketsSettings)
        {
            Bounds = outerBounds;
            BracketsSettings = bracketsSettings;
        }

    }
}
