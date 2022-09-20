using MatrixLayout.ExpressionDecorators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionLayout
{
    public interface IExpressionLayout
    {
        IComponentLayoutResult Layout(IExpressionComponent item);
    }

    public interface IComponentLayoutResult
    { }
}
