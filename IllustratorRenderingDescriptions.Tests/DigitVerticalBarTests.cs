using IllustratorRenderingDescriptions.NavyDigits.How;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.Tests
{
    public class DigitVerticalBarTests
    {
        [Test]
        public void Removes_TopLeft_Vertical_Bar()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var verticalBar = new DigitVerticalBar(DigitVerticalBarName.TopLeft, 0.1f);

            var verticalBarPoints = verticalBar.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(0, 10), new PointF(10, 10), new PointF(10, 45), new PointF(0, 45) }, verticalBarPoints);
        }

        [Test]
        public void Removes_TopRight_Vertical_Bar()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var verticalBar = new DigitVerticalBar(DigitVerticalBarName.TopRight, 0.1f);

            var verticalBarPoints = verticalBar.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(90, 10), new PointF(100, 10), new PointF(100, 45), new PointF(90, 45) }, verticalBarPoints);
        }

        [Test]
        public void Removes_BottomRight_Vertical_Bar()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var verticalBar = new DigitVerticalBar(DigitVerticalBarName.BottomRight, 0.1f);

            var verticalBarPoints = verticalBar.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(90, 55), new PointF(100, 55), new PointF(100, 90), new PointF(90, 90) }, verticalBarPoints);
        }

        [Test]
        public void Removes_BottomLeft_Vertical_Bar()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var verticalBar = new DigitVerticalBar(DigitVerticalBarName.BottomLeft, 0.1f);

            var verticalBarPoints = verticalBar.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(0, 55), new PointF(10, 55), new PointF(10, 90), new PointF(0, 90) }, verticalBarPoints);
        }
    }
}
