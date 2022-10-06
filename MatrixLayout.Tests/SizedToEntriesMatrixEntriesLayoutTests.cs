using Autofac.Extras.Moq;
using MatrixLayout.ExpressionLayout.LayoutResults;
using MatrixLayout.ExpressionLayout.Matrices;
using MatrixLayout.InputDescriptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.Tests
{
    public class SizedToEntriesMatrixEntriesLayoutTests
    {
        [Test]
        public void SingleEntryMatrixTakesUpTheSizeOfTheTextWhenThereIsNoPadding()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText("12", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(new MatrixInteriorMarginsDescription(0, 0, 0), 1, 1);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, new Font("Arial", 12), 12));

                Assert.AreEqual(new RectangleF(0, 0, 50, 35), results.GetEntryBounds(0, 0));
            }
        }

        [Test]
        public void SingleEntryMatrixWithoutBracketsUsesTheLeftOffsetCorrectly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText("12", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(new MatrixInteriorMarginsDescription(0, 0, 0), 1, 1);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, new Font("Arial", 12), 12), 99);

                Assert.AreEqual(new RectangleF(99, 0, 50, 35), results.GetEntryBounds(0, 0));
            }
        }

        [Test]
        public void SingleEntryMatrixWithBracketsUsesTheLeftOffsetCorrectly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText("12", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(new MatrixInteriorMarginsDescription(0, 0, 0), 1, 1);
                var results = (MatrixLayoutResults)layout.GetLayoutResultWithBrackets(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, new Font("Arial", 12), 12), new MatrixBracketsDescription(5, 20), 99);
                
                var entriesResult = results.GetComponents().OfType<MatrixEntriesLayoutResult>().First();
                Assert.AreEqual(new RectangleF(104, 0, 50, 35), entriesResult.GetEntryBounds(0, 0));
            }
        }

        [Test]
        public void SingleEntryMatrixWithOuterPaddingTakesUpTheEntireSpaceMinusTheOuterPadding()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText("12", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("0", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(new MatrixInteriorMarginsDescription(0.10f, 0, 0), 1, 1);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, new Font("Arial", 12), 12));

                Assert.AreEqual(new RectangleF(3.5f, 0, 50, 35), results.GetEntryBounds(0, 0));
            }
        }

        [Test]
        public void OneByThreeMatrixWithOuterPaddingAndColumnGapWorksCorrectly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText("12", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("3", It.IsAny<Font>()))
                    .Returns(new SizeF(20, 33));

                textMeasurer
                    .Setup(x => x.MeasureText("14", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 33));

                textMeasurer
                    .Setup(x => x.MeasureText("0", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(new MatrixInteriorMarginsDescription(0.10f, 0, 0.10f), 1, 3);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, new Font("Arial", 12), 12, 3, 14));

                Assert.AreEqual(new RectangleF(3.5f, 0, 50, 35), results.GetEntryBounds(0, 0));
                Assert.AreEqual(new RectangleF(57, 0, 20, 35), results.GetEntryBounds(0, 1));
                Assert.AreEqual(new RectangleF(80.5f, 0, 50, 35), results.GetEntryBounds(0, 2));
            }
        }

        [Test]
        public void ThreeByOneMatrixWithOuterPaddingAndRowGapWorksCorrectly()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText("12", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("3", It.IsAny<Font>()))
                    .Returns(new SizeF(20, 33));

                textMeasurer
                    .Setup(x => x.MeasureText("14", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 33));

                textMeasurer
                    .Setup(x => x.MeasureText("0", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(new MatrixInteriorMarginsDescription(0.10f, 0.10f, 0), 3, 1);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, new Font("Arial", 12), 12, 3, 14));

                Assert.AreEqual(new RectangleF(3.5f, 0, 50, 35), results.GetEntryBounds(0, 0));
                Assert.AreEqual(new RectangleF(3.5f, 38.5f, 50, 33), results.GetEntryBounds(1, 0));
                Assert.AreEqual(new RectangleF(3.5f, 75, 50, 33), results.GetEntryBounds(2, 0));
            }
        }

        [Test]
        public void TwoByTwoMatrixEntriesHaveTheCorrectBoundingBox()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText("1", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("12", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("34", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("5", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("0", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(new MatrixInteriorMarginsDescription(0, 0, 0), 2, 2);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, new Font("Arial", 12), 1, 12, 34, 5));

                var boundingBox = results.BoundingBox;
                Assert.AreEqual(new RectangleF(0, 0, 100, 70), boundingBox);
            }
        }

        [Test]
        public void SingleEntryMatrixWithBrackets()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText("12", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("0", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(new MatrixInteriorMarginsDescription(0, 0, 0), 1, 1);
                var results = (MatrixLayoutResults)layout.GetLayoutResultWithBrackets(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, new Font("Arial", 12), 12), new MatrixBracketsDescription(1, 20));

                var entriesResult = results.GetComponents().OfType<MatrixEntriesLayoutResult>().First();
                Assert.AreEqual(new RectangleF(1, 0, 50, 35), entriesResult.GetEntryBounds(0, 0));
            }
        }

        [Test]
        public void TwoByTwoMatrixWithBracketsHasTheCorrectBoundingBox()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText("1", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("12", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("34", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("5", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                textMeasurer
                    .Setup(x => x.MeasureText("0", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(new MatrixInteriorMarginsDescription(1, 0, 0), 2, 2);
                var results = layout.GetLayoutResultWithBrackets(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, new Font("Arial", 12), 1, 12, 34, 5), new MatrixBracketsDescription(3, 20));

                var boundingBox = results.BoundingBox;
                Assert.AreEqual(new RectangleF(0, 0, 176, 70), boundingBox);
            }
        }

        [Test]
        public void FindsTheMaximumValueInEachColumn()
        {
            var combiner = new MatrixEntriesSizeCombiner();
            var results = combiner.GetMaxForEachColumn(new List<float>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3);

            CollectionAssert.AreEqual(new[] { 7, 8, 9 }, results);
        }

        [Test]
        public void FindsTheMaximumValueInEachRow()
        {
            var combiner = new MatrixEntriesSizeCombiner();
            var results = combiner.GetMaxForEachRow(new List<float>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3);

            CollectionAssert.AreEqual(new[] { 3, 6, 9 }, results);
        }
    }
}
