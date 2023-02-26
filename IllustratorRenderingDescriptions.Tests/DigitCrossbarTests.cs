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
                new PointD(10, 45),
                new PointD(90, 45),
                new PointD(90, 55),
                new PointD(10, 55)
            };

            CollectionAssert.AreEqual(expectedResult, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Extends_Cross_Bar_To_The_Left()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var crossBar = new DigitCrossBar(0.1f) { ExtendLeft = true };

            var actualResult = crossBar.GetPoints(digitBoundingBox);

            var expectedResult = new[]
            {
                new PointD(0, 45),
                new PointD(90, 45),
                new PointD(90, 55),
                new PointD(0, 55)
            };

            CollectionAssert.AreEqual(expectedResult, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Leaves_Cross_Bar_Right_Padding()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var crossBar = new DigitCrossBar(0.1f) { RightPadding = 0.2f };

            var actualResult = crossBar.GetPoints(digitBoundingBox);

            var expectedResult = new[]
            {
                new PointD(10, 45),
                new PointD(74, 45),
                new PointD(74, 55),
                new PointD(10, 55)
            };

            CollectionAssert.AreEqual(expectedResult, actualResult.SelectMany(x => x.Points));
        }
    }
}
