using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class Expression : IExpressionComponent
    {
        public Expression(IExpressionComponent lhs, IExpressionComponent rhs)
        {

        }
    }

    public interface IExpressionComponent
    { }
}
