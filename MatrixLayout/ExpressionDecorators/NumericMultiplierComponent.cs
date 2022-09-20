using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class NumericMultiplierComponent : IExpressionComponent, ILeftMultipliableComponent, IAddableComponent
    {
        public readonly double Mult;
        public readonly INumericMultiplierCapableComponent Target;

        public NumericMultiplierComponent(double mult, INumericMultiplierCapableComponent target)
        {
            Mult = mult;
            Target = target;
        }
    }

    public interface INumericMultiplierCapableComponent : IExpressionComponent
    {

    }
}
