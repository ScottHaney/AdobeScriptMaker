using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class Equation : IEquationComponent
    {
        public readonly IExpressionComponent Lhs;
        public readonly IExpressionComponent Rhs;

        public Equation(IExpressionComponent lhs, IExpressionComponent rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }
    }

    public interface IEquationComponent : IExpressionComponent
    { }
}
