using Geometry;
using IllustratorRenderingDescriptions.NavyDigits.How;
using NUnit.Framework;
using System;
using System.Drawing;
using System.Linq;

namespace IllustratorRenderingDescriptions.Tests
{
    public class DigitCornerTests
    {
        [Test]
        public void Creates_Top_Left_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopLeft, 0.5f, 45);

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(0, 50), new PointF(0, 0), new PointF(50, 0) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Top_Left_Corner_Casts_Shadows_Correctly()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopLeft, 0.5f, 45);

            var results = corner.GetPoints(digitBoundingBox);

            var shadowsCreator = new DigitShadowLinesCreator() { IncludeMarble = false };
            var shadowLines = shadowsCreator.CreateShadows(digitBoundingBox, results.ToList());

            CollectionAssert.AreEqual(Array.Empty<Line>(), shadowLines);
        }

        [Test]
        public void Creates_Centered_Top_Left_Corner_In_A_Square_At_10_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopLeft, 0.1f, 45) { MoveToCenter = true };

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(0, 55), new PointF(0, 45), new PointF(10, 45) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Creates_Top_Right_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopRight, 0.5f, 45);

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(50, 0), new PointF(100, 0), new PointF(100, 50) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Creates_Centered_Top_Right_Corner_In_A_Square_At_10_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopRight, 0.1f, 45) { MoveToCenter = true };

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(90, 45), new PointF(100, 45), new PointF(100, 55) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Creates_Bottom_Right_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomRight, 0.5f, 45);

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(100, 50), new PointF(100, 100), new PointF(50, 100) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Creates_Centered_Bottom_Right_Corner_In_A_Square_At_10_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomRight, 0.1f, 45) { MoveToCenter = true };

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(100, 45), new PointF(100, 55), new PointF(90, 55) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Creates_Bottom_Left_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomLeft, 0.5f, 45);

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(50, 100), new PointF(0, 100), new PointF(0, 50) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Creates_Centered_Bottom_Left_Corner_In_A_Square_At_10_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomLeft, 0.1f, 45) { MoveToCenter = true };

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(10, 55), new PointF(0, 55), new PointF(0, 45) }, actualResult.SelectMany(x => x.Points));
        }
    }
}