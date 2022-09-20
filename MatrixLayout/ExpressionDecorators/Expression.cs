using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class Expression : IExpressionComponent, IEquationComponent
    {
        public Expression(params IExpressionComponent[] components)
        {

        }
    }

    public interface IExpressionComponent
    { }
}
