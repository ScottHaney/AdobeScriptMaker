using Geometry.Lines;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Tests
{
    public class SlopeTests
    {
        [Test]
        public void Two_Point_Slope_Has_The_Same_Slope_Regardless_Of_The_Order_Of_The_Points()
        {
            var point1 = new PointD(1, 1);
            var point2 = new PointD(5, 5);

            var slope1 = new TwoValueSlope(point1, point2);
            var slope2 = new TwoValueSlope(point2, point1);

            Assert.AreEqual(1, slope1.GetValue());
            Assert.IsTrue(slope1.GetValue() == slope2.GetValue());
        }
    }
}
