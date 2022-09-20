using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class AddComponents : IAddableComponent, IExpressionComponent
    {
        public AddComponents(IAddableComponent lhs, IAddableComponent rhs)
        { }
    }

    public interface IAddableComponent
    {

    }
}
