using NUnit.Framework;
using System.Drawing;

namespace Geometry.Tests
{
    public class LineTests
    {
        [Test]
        public void Slope_Of_A_Vertical_Line_Is_Null()
        {
            var line = new Line(new PointF(0, 1), new PointF(0, 10));

            Assert.AreEqual(null, line.GetSlope());
        }
    }
}