using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;

namespace MatrixLayout.Tests
{
    public class MatrixLayoutTests
    {
        [Test]
        public void SingleEntryMatrixWithNoPaddingTakesUpTheEntireSpace()
        {
            var layout = new MatrixEntriesLayout(0, 0, 0, 1, 1);
            var results = layout.GetLayoutResult(new RectangleF(0, 0, 100, 100));

            Assert.AreEqual(new RectangleF(0, 0, 100, 100), results.GetEntryBounds(0, 0));
        }

        [Test]
        public void SingleEntryMatrixWithOuterPaddingTakesUpTheEntireSpaceMinusTheOuterPadding()
        {
            var layout = new MatrixEntriesLayout(0.10f, 0, 0, 1, 1);
            var results = layout.GetLayoutResult(new RectangleF(0, 0, 100, 100));

            Assert.AreEqual(new RectangleF(10, 10, 80, 80), results.GetEntryBounds(0, 0));
        }

        [Test]
        public void OneByTwoMatrixWithOuterPaddingAndColumnGapWorksCorrectly()
        {
            var layout = new MatrixEntriesLayout(0.10f, 0, 0.10f, 1, 2);
            var results = layout.GetLayoutResult(new RectangleF(0, 0, 100, 100));

            Assert.AreEqual(new RectangleF(10, 10, 35, 80), results.GetEntryBounds(0, 0));
            Assert.AreEqual(new RectangleF(55, 10, 35, 80), results.GetEntryBounds(0, 1));
        }

        [Test]
        public void TwoByOneMatrixWithOuterPaddingAndRowGapWorksCorrectly()
        {
            var layout = new MatrixEntriesLayout(0.10f, 0.10f, 0, 2, 1);
            var results = layout.GetLayoutResult(new RectangleF(0, 0, 100, 100));

            Assert.AreEqual(new RectangleF(10, 10, 80, 35), results.GetEntryBounds(0, 0));
            Assert.AreEqual(new RectangleF(10, 55, 80, 35), results.GetEntryBounds(0, 1));
        }

        [Test]
        public void FindsTheMaximumValueInEachColumn()
        {
            var combiner = new MatrixEntriesSizeCombiner();
            var results = combiner.GetMaxForEachColumn(new List<float>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3);

            CollectionAssert.AreEqual(new[] { 7, 8, 9 }, results);
        }
    }
}