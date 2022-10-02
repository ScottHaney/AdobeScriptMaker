using Autofac.Extras.Moq;
using MatrixLayout.ExpressionDecorators;
using MatrixLayout.ExpressionLayout;
using MatrixLayout.ExpressionLayout.LayoutResults;
using MatrixLayout.InputDescriptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.Tests.ExpressionLayout
{
    public class MatrixExpressionLayoutTests
    {
        [Test]
        public void NumericMultiplierComponent_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText(It.IsAny<string>(), It.IsAny<Font>()))
                    .Returns(new SizeF(20, 35));

                var textMeasurerFactory = mock.Mock<ITextMeasurerFactory>();
                textMeasurerFactory
                    .Setup(x => x.Create())
                    .Returns(textMeasurer.Object);

                var layout = new MatrixExpressionLayout(
                    new TextDisplayDescription("Arial", 72),
                    new MatrixLayoutDescription(
                        new MatrixBracketsDescription(3, 20),
                        new MatrixInteriorMarginsDescription(0.10f, 0, 0)),
                    textMeasurerFactory.Object);

                var result = layout.Layout(new NumericMultiplierComponent(3, new MatrixComponent(3, 1, 1, 1, 1)));

                var innerResults = result.GetResults().ToList();

                var multiplierResult = innerResults.OfType<TextLayoutResult>().Single();
                Assert.AreEqual(new RectangleF(0, 70, 20, 35), multiplierResult.Bounds);
                Assert.AreEqual("3", multiplierResult.Text);

                var composite = (LayoutResultsComposite)result;
                var matrixResults = composite.Items.OfType<LayoutResultsComposite>().Single();

                Assert.AreEqual(new RectangleF(25, 35, 33, 105), matrixResults.BoundingBox);
            } 
        }

        [Test]
        public void AddComponents_Test()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText(It.IsAny<string>(), It.IsAny<Font>()))
                    .Returns(new SizeF(20, 35));

                var textMeasurerFactory = mock.Mock<ITextMeasurerFactory>();
                textMeasurerFactory
                    .Setup(x => x.Create())
                    .Returns(textMeasurer.Object);

                var layout = new MatrixExpressionLayout(
                    new TextDisplayDescription("Arial", 72),
                    new MatrixLayoutDescription(
                        new MatrixBracketsDescription(3, 20),
                        new MatrixInteriorMarginsDescription(0.10f, 0, 0)),
                    textMeasurerFactory.Object);

                var expression = new AddComponents(new MatrixComponent(3, 1, 1, 1, 1),
                        new MatrixComponent(3, 1, 1, 1, 1));

                var result = layout.Layout(expression);

                var innerResults = result.GetResults().ToList();
            }
        }
    }
}
