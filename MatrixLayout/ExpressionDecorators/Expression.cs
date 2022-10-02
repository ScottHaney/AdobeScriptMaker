using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class Expression : IExpressionComponent, IEquationComponent
    {
        public readonly IExpressionComponent[] Components;

        public Expression(params IExpressionComponent[] components)
        {
            Components = components ?? Array.Empty<IExpressionComponent>();
        }
    }

    public interface IExpressionComponent
    { }
}
