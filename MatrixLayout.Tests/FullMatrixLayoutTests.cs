using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout.Tests
{
    public class FullMatrixLayoutTests
    {
        [Test]
        public void SingleEntryMatrixWithBrackets()
        {
            var layout = new FullMatrixLayout(new UniformlySizedMatrixEntriesLayout(0, 0, 0, 1, 1), 1);
            var results = layout.GetLayoutResult(new RectangleF(0, 0, 100, 100));

            Assert.AreEqual(new RectangleF(1, 1, 98, 98), results.GetEntryBounds(0, 0));
        }
    }
}
