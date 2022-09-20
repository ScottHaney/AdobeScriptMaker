using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class MultiplyComponents : IAddableComponent, IOutsideMultiplierCapableComponent, IExpressionComponent
    {
        public MultiplyComponents(IMultipliableComponent lhs, IMultipliableComponent rhs)
        {

        }
    }

    public interface IMultipliableComponent
    { }
}
