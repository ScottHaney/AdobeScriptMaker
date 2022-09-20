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
            var expression = new Expression(
                new MultiplyComponents(
                    new MatrixComponent(1, 1, 1),
                    new MatrixComponent(1, 1, 1)));
        }

        [Test]
        public void CanRepresentMultiplyingAMatrixByANumber()
        {
            var expression = new Expression(
                new NumericMultiplierComponent(2, new MatrixComponent(1, 1, 1)));
        }

        [Test]
        public void CanRepresentAddingTwoMatricesThatHaveNumericMultipliers()
        {
            var expression = new Expression(
                new AddComponents(
                    new NumericMultiplierComponent(2, new MatrixComponent(1, 1, 1)),
                    new NumericMultiplierComponent(3, new MatrixComponent(1, 1, 1))));
        }

        [Test]
        public void CanRepresentAnEquationForMultiplyingTwoMatrices()
        {
            var lhs = new Expression(new MultiplyComponents(
                new MatrixComponent(1, 1, 1),
                new MatrixComponent(1, 1, 1)));

            var rhs = new Expression(
                new MatrixComponent(1, 1, 1));

            var equation = new Equation(lhs, rhs);
        }

        [Test]
        public void CanRepresentMultipleEqualsSignsOnTheSameLine_MultipleEquations()
        {
            var leftPart = new Expression(new MatrixComponent(1, 1, 1));
            var middlePart = new Expression(new MatrixComponent(1, 1, 1));
            var rightPart = new Expression(new MatrixComponent(1, 1, 1));

            var equation = new Equation(
                new Equation(leftPart, middlePart),
                rightPart);
        }
    }
}
