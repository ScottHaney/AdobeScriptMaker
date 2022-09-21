using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class AddComponents : IAddableComponent, IExpressionComponent
    {
        public readonly IAddableComponent Lhs;
        public readonly IAddableComponent Rhs;

        public AddComponents(IAddableComponent lhs, IAddableComponent rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }
    }

    public interface IAddableComponent
    {

    }
}
