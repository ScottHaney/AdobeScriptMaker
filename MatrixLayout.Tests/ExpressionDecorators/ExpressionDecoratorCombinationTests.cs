using MatrixLayout.ExpressionDecorators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.Tests.ExpressionDecorators
{
    public class ExpressionDecoratorCombinationTests
    {
        [Test]
        public void CanRepresentMultiplyingTwoMatrices()
        {
            var expression = new Expression(new MultiplyComponents(new MatrixComponent(), new MatrixComponent()));
        }

        [Test]
        public void CanRepresentMultiplyingAMatrixByANumber()
        {
            var expression = new Expression(new NumericMultiplierComponent(2, new MatrixComponent()));
        }

        [Test]
        public void CanRepresentAddingTwoMatricesThatHaveNumericMultipliers()
        {
            var expression = new Expression(
                new AddComponents(
                    new NumericMultiplierComponent(2, new MatrixComponent()),
                    new NumericMultiplierComponent(3, new MatrixComponent())));
        }
    }
}
