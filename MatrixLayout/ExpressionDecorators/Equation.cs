using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class Equation : IEquationComponent
    {
        public Equation(IEquationComponent lhs, IEquationComponent rhs)
        { }
    }

    public interface IEquationComponent : IExpressionComponent
    { }
}
