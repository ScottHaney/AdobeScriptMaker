using IllustratorRenderingDescriptions.NavyDigits.How;
using NUnit.Framework;
using System.Drawing;

namespace IllustratorRenderingDescriptions.Tests
{
    public class DigitSculptureTests
    {
        [Test]
        public void Carves_Out_Corners_From_A_Square()
        {
            var digitBoundingBox = new RectangleF(0, 0, 500, 800);
            var sculpture = new DigitSculpture(digitBoundingBox,
                new DigitCorner(DigitCornerName.TopLeft, 0.1f, 45),
                new DigitCorner(DigitCornerName.TopRight, 0.1f, 45),
                new DigitCorner(DigitCornerName.BottomRight, 0.1f, 45),
                new DigitCorner(DigitCornerName.BottomLeft, 0.1f, 45),
                new DigitHole(DigitHoleName.Top, 0.2f),
                new DigitHole(DigitHoleName.Bottom, 0.2f),
                new DigitCrossBar(0.2f));

            var script = sculpture.Carve();
        }
    }
}
