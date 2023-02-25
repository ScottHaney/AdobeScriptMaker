using Geometry.Lines;
using Geometry.LineSegments;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Tests
{
    public class LineSegmentTests
    {
        [Test]
        public void Two_Line_Segments_Are_Equal_If_They_Have_Their_Start_And_End_Points_The_Same_But_Reversed()
        {
            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());

            var p1 = new PointD(0, 0);
            var p2 = new PointD(1, 1);

            var segment = factory.Create(p1, p2);
            var reversedSegment = factory.Create(p2, p1);

            Assert.AreEqual(segment, reversedSegment);
        }
    }
}
