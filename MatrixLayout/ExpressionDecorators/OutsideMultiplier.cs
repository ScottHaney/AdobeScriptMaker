using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class OutsideMultiplier : IAddableComponent, IExpressionComponent
    {
        public OutsideMultiplier(IOutsideMultiplierCapableComponent matrixComponent)
        { }
    }

    public interface IOutsideMultiplierCapableComponent
    { }
}
