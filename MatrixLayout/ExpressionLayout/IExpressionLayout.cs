using MatrixLayout.ExpressionDecorators;
using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionLayout
{
    public interface IExpressionLayout
    {
        ILayoutResults Layout(IExpressionComponent item);
    }
}
