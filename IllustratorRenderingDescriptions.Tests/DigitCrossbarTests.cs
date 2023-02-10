using IllustratorRenderingDescriptions.NavyDigits.How;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.Tests
{
    public class DigitCrossbarTests
    {
        [Test]
        public void Creates_Cross_Bar()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var crossBar = new DigitCrossBar(0.1f);

            var actualResult = crossBar.GetPoints(digitBoundingBox);

            var expectedResult = new[]
            {
                new PointF(10, 45),
                new PointF(90, 45),
                new PointF(90, 55),
                new PointF(10, 55)
            };

            CollectionAssert.AreEqual(expectedResult, actualResult);
        }
    }
}
