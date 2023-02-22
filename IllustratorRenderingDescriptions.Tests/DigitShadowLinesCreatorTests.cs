using IllustratorRenderingDescriptions.NavyDigits.How;
using NUnit.Framework;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Geometry;

namespace IllustratorRenderingDescriptions.Tests
{
    public class DigitShadowLinesCreatorTests
    {
        [Test]
        public void Creates_Correct_Shadows_For_An_Unaltered_Piece_Of_Marble_With_A_Stroke()
        {
            var marble = new RectangleF(0, 0, 100, 100);

            var shadowLinesCreator = new DigitShadowLinesCreator() { StrokeWidth = 1 };
            var shadows = shadowLinesCreator.CreateShadows(marble, new List<DigitChiselResult>());
        }

        [Test]
        public void Creates_Correct_Shadows_For_The_Digit_One()
        {
            var marble = new RectangleF(0, 0, 100, 100);

            var chisler = new DigitOneChisler(0.2f);
            var chislerResults = chisler.GetPoints(marble);

            var shadowLinesCreator = new DigitShadowLinesCreator() { StrokeWidth = 1 };
            var shadows = shadowLinesCreator.CreateShadows(marble, chislerResults.ToList());
        }

        [Test]
        public void GetLinesTest()
        {
            
            var shadowLinesCreator = new DigitShadowLinesCreator() { StrokeWidth = 10 };
            var lines = shadowLinesCreator.GetLines(new Line(new PointF(-1, 1), new PointF(0, 2)), (float)Math.Sqrt(2));
        }

        /*[Test]
        public void Removing_The_Lower_Left_Vertical_Bar_And_Digit_Hole_Leaves_Only_A_Top_Shadow()
        {
            var withOverhang = GetResultsWithOverhang().ToList();
            var withoutOverhang = GetResultsWithoutOverhang().ToList();

            Assert.AreEqual(1, withOverhang.Count);
        }

        private IEnumerable<Geometry.Line> GetResultsWithoutOverhang()
        {
            var widthPaddingPercentage = 0.2f;
            var marble = new RectangleF(0, 0, 1000, 1000);

            var chiselActions = new IDigitChisleAction[]
            {
                new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = 0 },
                new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage)
            };

            var chiselResults = chiselActions.SelectMany(x => x.GetPoints(marble));

            var linesCreator = new DigitShadowLinesCreator();
            return linesCreator.CreateShadows(marble, chiselResults.ToList()).ToList();
        }

        private IEnumerable<Geometry.Line> GetResultsWithOverhang()
        {
            var widthPaddingPercentage = 0.2f;
            var marble = new RectangleF(0, 0, 1000, 1000);

            var chiselActions = new IDigitChisleAction[]
            {
                new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = 0.1f },
                new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage)
            };

            var chiselResults = chiselActions.SelectMany(x => x.GetPoints(marble));

            var linesCreator = new DigitShadowLinesCreator();
            return linesCreator.CreateShadows(marble, chiselResults.ToList()).ToList();
        }*/
    }
}
