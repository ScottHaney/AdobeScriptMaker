using Geometry;
using Geometry.Lines;
using Geometry.LineSegments;
using IllustratorRenderingDescriptions.NavyDigits.How;
using IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions;
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

            CollectionAssert.AreEqual(new[] { new PointD(0, 50), new PointD(0, 0), new PointD(50, 0) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Top_Left_Corner_Casts_Shadows_Correctly()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopLeft, 0.5f, 45);

            var results = corner.GetPoints(digitBoundingBox);

            var shadowsCreator = new DigitShadowLinesCreator2() { IncludeMarble = false };
            var shadowLines = shadowsCreator.CreateShadows(digitBoundingBox, results.ToList());

            CollectionAssert.AreEquivalent(Array.Empty<LineSegment>(), shadowLines);
        }

        [Test]
        public void Creates_Centered_Top_Left_Corner_In_A_Square_At_10_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopLeft, 0.1f, 45) { MoveToCenter = true };

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointD(0, 55), new PointD(0, 45), new PointD(10, 45) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Centered_Top_Left_Corner_Casts_Shadows_Correctly()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopLeft, 0.1f, 45) { MoveToCenter = true };

            var results = corner.GetPoints(digitBoundingBox);

            var shadowsCreator = new DigitShadowLinesCreator2(new ShadowCreator(0.1f, 45)) { StrokeWidth = 0 };
            var shadowLines = shadowsCreator.CreateShadows(digitBoundingBox, results.ToList());

            var result = results.First();
            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());
            CollectionAssert.Contains(shadowLines, factory.Create(result.Points[1], result.Points[2]));
        }

        [Test]
        public void Creates_Top_Right_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopRight, 0.5f, 45);

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointD(50, 0), new PointD(100, 0), new PointD(100, 50) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Top_Right_Corner_Casts_Shadows_Correctly()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopRight, 0.5f, 45);

            var results = corner.GetPoints(digitBoundingBox);

            var shadowsCreator = new DigitShadowLinesCreator2() { IncludeMarble = false };
            var shadowLines = shadowsCreator.CreateShadows(digitBoundingBox, results.ToList());

            CollectionAssert.AreEquivalent(Array.Empty<LineSegment>(), shadowLines);
        }

        [Test]
        public void Creates_Centered_Top_Right_Corner_In_A_Square_At_10_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopRight, 0.1f, 45) { MoveToCenter = true };

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointD(90, 45), new PointD(100, 45), new PointD(100, 55) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Centered_Top_Right_Corner_Casts_Shadows_Correctly()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.TopRight, 0.1f, 45) { MoveToCenter = true };

            var results = corner.GetPoints(digitBoundingBox);

            var shadowsCreator = new DigitShadowLinesCreator2(new ShadowCreator(0.1f, 45)) { StrokeWidth = 0 };
            var shadowLines = shadowsCreator.CreateShadows(digitBoundingBox, results.ToList());

            var result = results.First();
            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());
            CollectionAssert.Contains(shadowLines, factory.Create(result.Points[0], result.Points[1]));
        }

        [Test]
        public void Creates_Bottom_Right_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomRight, 0.5f, 45);

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointD(100, 50), new PointD(100, 100), new PointD(50, 100) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Bottom_Right_Corner_Casts_Shadows_Correctly()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomRight, 0.5f, 45);

            var results = corner.GetPoints(digitBoundingBox);

            var shadowsCreator = new DigitShadowLinesCreator2(new ShadowCreator(0.5f, 45)) { StrokeWidth = 0 };
            var shadowLines = shadowsCreator.CreateShadows(digitBoundingBox, results.ToList());

            var result = results.First();
            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());
            CollectionAssert.Contains(shadowLines, factory.Create(result.Points[0], result.Points[2]));
        }

        [Test]
        public void Creates_Centered_Bottom_Right_Corner_In_A_Square_At_10_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomRight, 0.1f, 45) { MoveToCenter = true };

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointD(100, 45), new PointD(100, 55), new PointD(90, 55) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Centered_Bottom_Right_Corner_Casts_Shadows_Correctly()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomRight, 0.1f, 45) { MoveToCenter = true };

            var results = corner.GetPoints(digitBoundingBox);

            var shadowsCreator = new DigitShadowLinesCreator2(new ShadowCreator(0.1f, 45)) { StrokeWidth = 0 };
            var shadowLines = shadowsCreator.CreateShadows(digitBoundingBox, results.ToList());

            var result = results.First();
            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());
            CollectionAssert.Contains(shadowLines, factory.Create(result.Points[0], result.Points[2]));
        }

        [Test]
        public void Creates_Bottom_Left_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomLeft, 0.5f, 45);

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointD(50, 100), new PointD(0, 100), new PointD(0, 50) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Bottom_Left_Corner_Casts_Shadows_Correctly()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomLeft, 0.5f, 45);

            var results = corner.GetPoints(digitBoundingBox);

            var shadowsCreator = new DigitShadowLinesCreator2() { IncludeMarble = false };
            var shadowLines = shadowsCreator.CreateShadows(digitBoundingBox, results.ToList());

            CollectionAssert.AreEquivalent(Array.Empty<LineSegment>(), shadowLines);
        }

        [Test]
        public void Creates_Centered_Bottom_Left_Corner_In_A_Square_At_10_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomLeft, 0.1f, 45) { MoveToCenter = true };

            var actualResult = corner.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointD(10, 55), new PointD(0, 55), new PointD(0, 45) }, actualResult.SelectMany(x => x.Points));
        }

        [Test]
        public void Centered_Bottom_Left_Corner_Casts_Shadows_Correctly()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(DigitCornerName.BottomLeft, 0.1f, 45) { MoveToCenter = true };

            var results = corner.GetPoints(digitBoundingBox);

            var shadowsCreator = new DigitShadowLinesCreator2() { IncludeMarble = false };
            var shadowLines = shadowsCreator.CreateShadows(digitBoundingBox, results.ToList());

            CollectionAssert.AreEquivalent(Array.Empty<LineSegment>(), shadowLines);
        }
    }
}