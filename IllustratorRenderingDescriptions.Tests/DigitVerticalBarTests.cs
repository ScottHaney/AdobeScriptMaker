﻿using IllustratorRenderingDescriptions.NavyDigits.How;
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
            var digitBoundingBox = new RectangleF(1000, 1000, 100, 100);
            var verticalBar = new DigitVerticalBar(DigitVerticalBarName.TopLeft, 0.1f);

            var verticalBarPoints = verticalBar.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(1000, 1010), new PointF(1010, 1010), new PointF(1010, 1045), new PointF(1000, 1045) }, verticalBarPoints);
        }

        [Test]
        public void Removes_TopRight_Vertical_Bar()
        {
            var digitBoundingBox = new RectangleF(1000, 1000, 100, 100);
            var verticalBar = new DigitVerticalBar(DigitVerticalBarName.TopRight, 0.1f);

            var verticalBarPoints = verticalBar.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(1090, 1010), new PointF(1100, 1010), new PointF(1100, 1045), new PointF(1090, 1045) }, verticalBarPoints);
        }

        [Test]
        public void Removes_BottomRight_Vertical_Bar()
        {
            var digitBoundingBox = new RectangleF(1000, 1000, 100, 100);
            var verticalBar = new DigitVerticalBar(DigitVerticalBarName.BottomRight, 0.1f);

            var verticalBarPoints = verticalBar.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(1090, 1055), new PointF(1100, 1055), new PointF(1100, 1090), new PointF(1090, 1090) }, verticalBarPoints);
        }

        [Test]
        public void Removes_BottomLeft_Vertical_Bar()
        {
            var digitBoundingBox = new RectangleF(1000, 1000, 100, 100);
            var verticalBar = new DigitVerticalBar(DigitVerticalBarName.BottomLeft, 0.1f);

            var verticalBarPoints = verticalBar.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(1000, 1055), new PointF(1010, 1055), new PointF(1010, 1090), new PointF(1000, 1090) }, verticalBarPoints);
        }
    }
}
