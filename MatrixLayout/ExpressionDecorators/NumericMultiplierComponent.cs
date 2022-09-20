using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class NumericMultiplierComponent : IExpressionComponent, ILeftMultipliableComponent, IAddableComponent
    {
        public NumericMultiplierComponent(double mult, INumericMultiplierCapableComponent matrix)
        {

        }
    }

    public interface INumericMultiplierCapableComponent
    {

    }
}
