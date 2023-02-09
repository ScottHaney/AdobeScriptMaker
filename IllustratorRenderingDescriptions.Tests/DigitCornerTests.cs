using IllustratorRenderingDescriptions.NavyDigits.How;
using NUnit.Framework;
using System.Drawing;

namespace IllustratorRenderingDescriptions.Tests
{
    public class DigitCornerTests
    {
        [Test]
        public void Creates_Top_Left_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(0.5f, 45);

            var topLeftPoints = corner.GetPoints(DigitCornerName.TopLeft, digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(0, 50), new PointF(0, 0), new PointF(50, 0) }, topLeftPoints);
        }

        [Test]
        public void Creates_Top_Right_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(0.5f, 45);

            var topRightPoints = corner.GetPoints(DigitCornerName.TopRight, digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(50, 0), new PointF(100, 0), new PointF(100, 50) }, topRightPoints);
        }

        [Test]
        public void Creates_Bottom_Right_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(0.5f, 45);

            var bottomRightPoints = corner.GetPoints(DigitCornerName.BottomRight, digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(100, 50), new PointF(100, 100), new PointF(50, 100) }, bottomRightPoints);
        }

        [Test]
        public void Creates_Bottom_Left_Corner_In_A_Square_At_45_Degrees()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(0.5f, 45);

            var bottomLeftPoints = corner.GetPoints(DigitCornerName.BottomLeft, digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(50, 100), new PointF(0, 100), new PointF(0, 50) }, bottomLeftPoints);
        }
    }
}