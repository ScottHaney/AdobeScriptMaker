using Autofac.Extras.Moq;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout.Tests
{
    public class SizedToEntriesMatrixEntriesLayoutTests
    {
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

        [Test]
        public void SingleEntryMatrixTakesUpTheSizeOfTheTextWhenThereIsNoPadding()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var textMeasurer = mock.Mock<ITextMeasurer>();
                textMeasurer
                    .Setup(x => x.MeasureText("12", It.IsAny<Font>()))
                    .Returns(new SizeF(50, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(0, 0, 0, 1, 1);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, null, 12));

                Assert.AreEqual(new RectangleF(0, 0, 50, 35), results.GetEntryBounds(0, 0));
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

                var layout = new SizedToEntriesMatrixEntriesLayout(0.10f, 0, 0, 1, 1);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, null, 12));

                Assert.AreEqual(new RectangleF(3.5f, 3.5f, 50, 35), results.GetEntryBounds(0, 0));
            }
        }

        [Test]
        public void OneByTwoMatrixWithOuterPaddingAndColumnGapWorksCorrectly()
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
                    .Setup(x => x.MeasureText("0", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(0.10f, 0, 0.10f, 1, 2);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, null, 12, 3));

                Assert.AreEqual(new RectangleF(3.5f, 3.5f, 50, 35), results.GetEntryBounds(0, 0));
                Assert.AreEqual(new RectangleF(57, 3.5f, 20, 35), results.GetEntryBounds(0, 1));
            }
        }

        [Test]
        public void TwoByOneMatrixWithOuterPaddingAndRowGapWorksCorrectly()
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
                    .Setup(x => x.MeasureText("0", It.IsAny<Font>()))
                    .Returns(new SizeF(25, 35));

                var layout = new SizedToEntriesMatrixEntriesLayout(0.10f, 0.10f, 0, 2, 1);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, null, 12, 3));

                Assert.AreEqual(new RectangleF(3.5f, 3.5f, 50, 35), results.GetEntryBounds(0, 0));
                Assert.AreEqual(new RectangleF(3.5f, 42, 50, 33), results.GetEntryBounds(1, 0));
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

                var layout = new SizedToEntriesMatrixEntriesLayout(0, 0, 0, 2, 2);
                var results = layout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, null, 1, 12, 34, 5));

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

                var layout = new SizedToEntriesMatrixEntriesLayout(0, 0, 0, 1, 1);
                var results = layout.GetLayoutResultWithBrackets(new SizedMatrixEntriesLayoutInputParams(textMeasurer.Object, null, 12), 1);

                Assert.AreEqual(new RectangleF(1, 1, 50, 35), results.GetEntryBounds(0, 0));
            }
        }
    }
}
