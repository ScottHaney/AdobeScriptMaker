using MatrixLayout.ExpressionDecorators;
using MatrixLayout.ExpressionLayout;
using MatrixLayout.InputDescriptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixLayout.Tests.ExpressionLayout
{
    public class MatrixExpressionLayoutTests
    {
        [Test]
        public void NumericMultiplierComponent_Test()
        {
            var layout = new MatrixExpressionLayout(
                new TextDisplayDescription("Arial", 72),
                new MatrixLayoutDescription(
                    new MatrixBracketsDescription(3, 20),
                    new MatrixInteriorMarginsDescription(0.10f, 0, 0)));

            var result = layout.Layout(new NumericMultiplierComponent(3, new MatrixComponent(3, 1, 1, 1, 1)));

            var innerResults = result.GetResults().ToList();
        }
    }
}
