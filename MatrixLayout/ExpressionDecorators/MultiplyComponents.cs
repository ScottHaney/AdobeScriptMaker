using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class MultiplyComponents : IAddableComponent, INumericMultiplierCapableComponent, IExpressionComponent
    {
        public MultiplyComponents(ILeftMultipliableComponent lhs, IRightMultipliableComponent rhs)
        {

        }
    }

    public interface ILeftMultipliableComponent
    { }

    public interface IRightMultipliableComponent
    { }

    public interface IMultipliableComponent : ILeftMultipliableComponent, IRightMultipliableComponent
    { }
}
