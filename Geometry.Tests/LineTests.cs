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

        [Test]
        public void Slope_Of_A_45_Degree_Line_Is_One()
        {
            var line = new Line(new PointF(0, 0), new PointF(1, 1));

            Assert.AreEqual(1, line.GetSlope());
        }
    }
}