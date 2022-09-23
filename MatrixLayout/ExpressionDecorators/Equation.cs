using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class Equation : IEquationComponent
    {
        public readonly IEquationComponent Lhs;
        public readonly IEquationComponent Rhs;

        public Equation(IEquationComponent lhs, IEquationComponent rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }
    }

    public interface IEquationComponent : IExpressionComponent
    { }
}
