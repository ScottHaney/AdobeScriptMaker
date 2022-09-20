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
                var results = layout.GetLayoutResult(textMeasurer.Object, null, 12);

                Assert.AreEqual(new RectangleF(0, 0, 50, 35), results.GetEntryBounds(0, 0));
            }
        }
    }
}
