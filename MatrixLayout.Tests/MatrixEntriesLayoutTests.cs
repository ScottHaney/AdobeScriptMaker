using NUnit.Framework;
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
    }
}