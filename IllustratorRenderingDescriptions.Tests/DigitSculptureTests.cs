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
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var corner = new DigitCorner(0.1f, 45);

            var sculpture = new DigitSculpture(digitBoundingBox, corner);
            var actualResult = sculpture.Carve();

            var expectedResult = new[]
            {
                new PointF(0, 10),
                new PointF(10, 0),
                new PointF(90, 0),
                new PointF(100, 10),
                new PointF(100, 90),
                new PointF(90, 100),
                new PointF(10, 100),
                new PointF(0, 90)
            };

            CollectionAssert.AreEqual(expectedResult, actualResult);
        }
    }
}
