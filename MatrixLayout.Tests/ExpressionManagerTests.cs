using MatrixLayout.ExpressionDecorators;
using MatrixLayout.InputDescriptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.Tests
{
    public class ExpressionManagerTests
    {
        [Test]
        public void Test()
        {
            var expressionManager = new ExpressionManager(new ExpressionDisplaySettings(
                new TextDisplayDescription("Arial", 12),
                new MatrixLayoutDescription(
                    new MatrixBracketsDescription(2, 0.15f),
                    new MatrixInteriorMarginsDescription(0.10f, 0.8f, 0.5f))));

            var expression = new MatrixComponent(new MatrixValuesDescription(2, 2, 1, 2, 3, 4));

            var result = expressionManager.Render(expression);
        }
    }
}
