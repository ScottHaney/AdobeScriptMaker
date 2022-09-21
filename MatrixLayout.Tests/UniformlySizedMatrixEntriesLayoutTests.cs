using MatrixLayout.ExpressionLayout.Matrices;
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
            var layout = new UniformlySizedMatrixEntriesLayout(0, 0, 0, 1, 1);
            var results = layout.GetLayoutResult(new UniformMatrixEntriesLayoutInputParams(new RectangleF(0, 0, 100, 100)));

            Assert.AreEqual(new RectangleF(0, 0, 100, 100), results.GetEntryBounds(0, 0));
        }

        [Test]
        public void SingleEntryMatrixWithOuterPaddingTakesUpTheEntireSpaceMinusTheOuterPadding()
        {
            var layout = new UniformlySizedMatrixEntriesLayout(0.10f, 0, 0, 1, 1);
            var results = layout.GetLayoutResult(new UniformMatrixEntriesLayoutInputParams(new RectangleF(0, 0, 100, 100)));

            Assert.AreEqual(new RectangleF(10, 10, 80, 80), results.GetEntryBounds(0, 0));
        }

        [Test]
        public void OneByTwoMatrixWithOuterPaddingAndColumnGapWorksCorrectly()
        {
            var layout = new UniformlySizedMatrixEntriesLayout(0.10f, 0, 0.10f, 1, 2);
            var results = layout.GetLayoutResult(new UniformMatrixEntriesLayoutInputParams(new RectangleF(0, 0, 100, 100)));

            Assert.AreEqual(new RectangleF(10, 10, 35, 80), results.GetEntryBounds(0, 0));
            Assert.AreEqual(new RectangleF(55, 10, 35, 80), results.GetEntryBounds(0, 1));
        }

        [Test]
        public void TwoByOneMatrixWithOuterPaddingAndRowGapWorksCorrectly()
        {
            var layout = new UniformlySizedMatrixEntriesLayout(0.10f, 0.10f, 0, 2, 1);
            var results = layout.GetLayoutResult(new UniformMatrixEntriesLayoutInputParams(new RectangleF(0, 0, 100, 100)));

            Assert.AreEqual(new RectangleF(10, 10, 80, 35), results.GetEntryBounds(0, 0));
            Assert.AreEqual(new RectangleF(10, 55, 80, 35), results.GetEntryBounds(0, 1));
        }

        [Test]
        public void SingleEntryMatrixWithBrackets()
        {
            var layout = new UniformlySizedMatrixEntriesLayout(0, 0, 0, 1, 1);
            var results = layout.GetLayoutResultWithBrackets(new UniformMatrixEntriesLayoutInputParams(new RectangleF(0, 0, 100, 100)), 1);

            Assert.AreEqual(new RectangleF(1, 1, 98, 98), results.GetEntryBounds(0, 0));
        }
    }
}