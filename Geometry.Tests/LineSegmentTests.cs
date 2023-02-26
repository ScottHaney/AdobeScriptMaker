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

            Assert.IsTrue(segment == reversedSegment);
            Assert.AreEqual(segment, reversedSegment);
        }

        [Test]
        public void Excluding_Two_Line_Segments_On_Different_Lines_Returns_The_Original_Line()
        {
            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());

            var segment1 = factory.Create(new PointD(0, 0), new PointD(1, 1));
            var segment2 = factory.Create(new PointD(0, 1), new PointD(0, 10));

            CollectionAssert.AreEqual(new[] { segment1 }, segment1.Exclude(segment2));
            CollectionAssert.AreEqual(new[] { segment2 }, segment2.Exclude(segment1));
        }

        [Test]
        public void Excluding_Two_Line_Segments_On_The_Same_Line_That_Overlap_Only_At_A_Single_EndPoint_Returns_The_Original_Line()
        {
            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());

            var segment1 = factory.Create(new PointD(0, 0), new PointD(1, 1));
            var segment2 = factory.Create(new PointD(1, 1), new PointD(2, 2));

            CollectionAssert.AreEqual(new[] { segment1 }, segment1.Exclude(segment2));
            CollectionAssert.AreEqual(new[] { segment2 }, segment2.Exclude(segment1));
        }

        [Test]
        public void Excluding_Two_Line_Segments_On_The_Same_Line_That_Overlap_Only_On_An_End_Interval_Works_Correctly()
        {
            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());

            var segment1 = factory.Create(new PointD(0, 0), new PointD(2, 2));
            var segment2 = factory.Create(new PointD(1, 1), new PointD(3, 3));

            CollectionAssert.AreEqual(new[] { factory.Create(new PointD(0, 0), new PointD(1, 1)) }, segment1.Exclude(segment2));
            CollectionAssert.AreEqual(new[] { factory.Create(new PointD(2, 2), new PointD(3, 3)) }, segment2.Exclude(segment1));
        }

        [Test]
        public void Excluding_Two_Line_Segments_On_The_Same_Line_That_Overlap_Internally_Works_Correctly()
        {
            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());

            var segment1 = factory.Create(new PointD(0, 0), new PointD(5, 5));
            var segment2 = factory.Create(new PointD(1, 1), new PointD(3, 3));

            CollectionAssert.AreEquivalent(new[]
            {
                factory.Create(new PointD(0, 0), new PointD(1, 1)),
                factory.Create(new PointD(3, 3), new PointD(5, 5))
            }, segment1.Exclude(segment2));
        }
    }
}
