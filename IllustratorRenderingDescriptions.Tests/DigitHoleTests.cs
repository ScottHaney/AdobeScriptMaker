using IllustratorRenderingDescriptions.NavyDigits.How;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.Tests
{
    public class DigitHoleTests
    {
        [Test]
        public void Removes_The_Top_Hole()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var hole = new DigitHole(0.1f);

            var actualResult = hole.GetPoints(DigitHoleName.Top, digitBoundingBox);

            var expectedResult = new[]
            {
                new PointF(10, 10),
                new PointF(90, 10),
                new PointF(90, 45),
                new PointF(10, 45)
            };

            CollectionAssert.AreEqual(expectedResult, actualResult);
        }
    }
}
