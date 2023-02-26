using IllustratorRenderingDescriptions.NavyDigits.How;
using NUnit.Framework;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Geometry;
using Geometry.Lines;
using Geometry.LineSegments;
using IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions;

namespace IllustratorRenderingDescriptions.Tests
{
    public class DigitShadowLinesCreatorTests
    {
        [Test]
        public void Creates_Correct_Shadows_For_An_Unaltered_Piece_Of_Marble_With_A_Stroke()
        {
            var marble = new RectangleF(0, 0, 100, 100);

            var shadowLinesCreator = new DigitShadowLinesCreator(new ShadowCreator(0.2f, 45)) { StrokeWidth = 0 };
            var shadowLines = shadowLinesCreator.CreateShadows(marble, new List<DigitChiselResult>());

            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());
            var expectedResult = new[]
            {
                factory.Create(new PointD(0, 100), new PointD(100, 100)),
                factory.Create(new PointD(100, 0), new PointD(100, 100))
            };

            CollectionAssert.AreEquivalent(expectedResult, shadowLines);
        }

        [Test]
        public void Creates_Correct_Shadows_For_Marble_With_The_Bottom_Right_Corner_Removed()
        {
            var marble = new RectangleF(0, 0, 100, 100);
            var widthPercentage = 0.2f;
            var angle = 45;

            var bottomRightCornerChisler = new DigitCorner(DigitCornerName.BottomRight, widthPercentage, angle);
            var result = bottomRightCornerChisler.GetPoints(marble);

            var shadowLinesCreator = new DigitShadowLinesCreator(new ShadowCreator(widthPercentage, angle)) { StrokeWidth = 0 };
            var shadowLines = shadowLinesCreator.CreateShadows(marble, result.ToList());

            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());
            var expectedResult = new[]
            {
                factory.Create(new PointD(0, 100), new PointD(80, 100)),
                factory.Create(new PointD(100, 0), new PointD(100, 80)),
                factory.Create(new PointD(80, 100), new PointD(100, 80))
            };

            CollectionAssert.AreEquivalent(expectedResult, shadowLines);
        }

        [Test]
        public void Creates_Correct_Shadows_For_Marble_With_The_Centered_Bottom_Right_Corner_Removed()
        {
            var marble = new RectangleF(0, 0, 100, 100);
            var widthPercentage = 0.2f;
            var angle = 45;

            var bottomRightCornerChisler = new DigitCorner(DigitCornerName.BottomRight, widthPercentage, angle) { MoveToCenter = true };
            var result = bottomRightCornerChisler.GetPoints(marble);

            var shadowLinesCreator = new DigitShadowLinesCreator(new ShadowCreator(widthPercentage, angle)) { StrokeWidth = 0 };
            var shadowLines = shadowLinesCreator.CreateShadows(marble, result.ToList());

            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());
            var expectedResult = new[]
            {
                factory.Create(new PointD(0, 100), new PointD(100, 100)),
                factory.Create(new PointD(100, 100), new PointD(100, 60)),
                factory.Create(new PointD(100, 40), new PointD(80, 60)),
                factory.Create(new PointD(100, 0), new PointD(100, 40))
            };

            CollectionAssert.AreEquivalent(expectedResult, shadowLines);
        }

        [Test]
        public void Creates_Correct_Shadows_For_Marble_With_The_Bottom_Hole_Removed()
        {
            var marble = new RectangleF(0, 0, 100, 100);
            var widthPercentage = 0.2f;
            var angle = 45;

            var results = new List<DigitChiselResult>();
            var bottomHoleChisler = new DigitHole(DigitHoleName.Bottom, widthPercentage, angle);
            results.AddRange(bottomHoleChisler.GetPoints(marble));

            var shadowLinesCreator = new DigitShadowLinesCreator(new ShadowCreator(widthPercentage, angle)) { StrokeWidth = 0 };
            var shadowLines = shadowLinesCreator.CreateShadows(marble, results);

            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());
            var expectedResult = new[]
            {
                factory.Create(new PointD(20, 60), new PointD(80, 60)),
                factory.Create(new PointD(20, 60), new PointD(20, 80)),
                factory.Create(new PointD(0, 100), new PointD(100, 100)),
                factory.Create(new PointD(100, 0), new PointD(100, 100))
            };

            CollectionAssert.AreEquivalent(expectedResult, shadowLines);
        }

        [Test]
        public void Creates_Correct_Shadows_For_Marble_With_The_Centered_Bottom_Right_Corner_Removed_And_Bottom_Hole_Removed()
        {
            var marble = new RectangleF(0, 0, 100, 100);
            var widthPercentage = 0.2f;
            var angle = 45;

            var results = new List<DigitChiselResult>();
            var bottomRightCornerChisler = new DigitCorner(DigitCornerName.BottomRight, widthPercentage, angle) { MoveToCenter = true };
            results.AddRange(bottomRightCornerChisler.GetPoints(marble));

            var bottomHoleChisler = new DigitHole(DigitHoleName.Bottom, widthPercentage, angle);
            results.AddRange(bottomHoleChisler.GetPoints(marble));

            var bottomBarChisler = new DigitVerticalBar(DigitVerticalBarName.BottomRight, widthPercentage);
            results.AddRange(bottomBarChisler.GetPoints(marble));

            var shadowLinesCreator = new DigitShadowLinesCreator(new ShadowCreator(widthPercentage, angle)) { StrokeWidth = 0 };
            var shadowLines = shadowLinesCreator.CreateShadows(marble, results);

            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());
            var expectedResult = new[]
            {
                factory.Create(new PointD(0, 100), new PointD(100, 100)),
                factory.Create(new PointD(100, 100), new PointD(100, 80)),
                factory.Create(new PointD(100, 40), new PointD(80, 60)),
                factory.Create(new PointD(100, 0), new PointD(100, 40)),
                factory.Create(new PointD(20, 60), new PointD(80, 60)),
                factory.Create(new PointD(20, 60), new PointD(20, 80))
            };

            CollectionAssert.AreEquivalent(expectedResult, shadowLines);
        }
    }
}
