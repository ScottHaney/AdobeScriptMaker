using Geometry;
using IllustratorRenderingDescriptions.NavyDigits.How;
using IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IllustratorRenderingDescriptions.Tests
{
    public class DigitHoleTests
    {
        [Test]
        public void Removes_The_Top_Hole()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var hole = new DigitHole(DigitHoleName.Top, 0.1f);

            var actualResult = hole.GetPoints(digitBoundingBox);

            var expectedResult = new[]
            {
                new PointD(10, 10),
                new PointD(90, 10),
                new PointD(90, 45),
                new PointD(10, 45)
            };

            CollectionAssert.AreEqual(expectedResult, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Removes_The_Bottom_Hole()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var hole = new DigitHole(DigitHoleName.Bottom, 0.1f);

            var actualResult = hole.GetPoints(digitBoundingBox);

            var expectedResult = new[]
            {
                new PointD(10, 55),
                new PointD(90, 55),
                new PointD(90, 90),
                new PointD(10, 90)
            };

            CollectionAssert.AreEqual(expectedResult, actualResult.SelectMany(x => x.Points));
        }
    }
}
