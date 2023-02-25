using Geometry.Intervals;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry.Tests
{
    public class IntervalTests
    {
        [Test]
        public void Throws_An_Exception_When_The_Start_And_End_Are_In_The_Wrong_Order()
        {
            Assert.Throws<InvalidIntervalException>(() => Interval.CreateOpenInterval(10, 5));
        }

        [Test]
        public void Report_An_Interval_With_One_Point_As_A_NonEmpty_Interval()
        {
            var onePointInterval = Interval.CreateClosedInterval(5, 5);
            Assert.IsFalse(onePointInterval.IsEmpty());
        }

        [Test]
        public void Report_An_Interval_With_Zero_Points_As_An_Empty_Interval()
        {
            var emptyIntervals = new[]
            {
                Interval.CreateOpenInterval(5, 5),
                new Interval(new OpenIntervalEndPoint(5), new ClosedIntervalEndPoint(5)),
                new Interval(new ClosedIntervalEndPoint(5), new OpenIntervalEndPoint(5))
            };

            foreach (var emptyInterval in emptyIntervals)
                Assert.IsTrue(emptyInterval.IsEmpty());
        }

        [Test]
        public void Intervals_That_Have_No_Points_In_Common_Have_Empty_Intersection_Results()
        {
            var interval1 = Interval.CreateOpenInterval(0, 5);
            var interval2 = Interval.CreateOpenInterval(10, 15);

            Assert.IsTrue(interval1.IntersectionWith(interval2).IsEmpty());
            Assert.IsTrue(interval2.IntersectionWith(interval1).IsEmpty());
        }

        public void Intervals_That_Overlap_At_Only_A_Single_EndPoint_Value_Not_Included_In_Both_Intervals_Have_Empty_Intersection_Results()
        {
            var interval1A = Interval.CreateOpenInterval(0, 5);
            var interval2A = Interval.CreateClosedInterval(5, 10);

            Assert.IsTrue(interval1A.IntersectionWith(interval2A).IsEmpty());
            Assert.IsTrue(interval2A.IntersectionWith(interval1A).IsEmpty());

            var interval1B = Interval.CreateClosedInterval(0, 5);
            var interval2B = Interval.CreateOpenInterval(5, 10);

            Assert.IsTrue(interval1B.IntersectionWith(interval2B).IsEmpty());
            Assert.IsTrue(interval2B.IntersectionWith(interval1B).IsEmpty());

            var interval1C = Interval.CreateOpenInterval(0, 5);
            var interval2C = Interval.CreateOpenInterval(5, 10);

            Assert.IsTrue(interval1C.IntersectionWith(interval2C).IsEmpty());
            Assert.IsTrue(interval2C.IntersectionWith(interval1C).IsEmpty());
        }

        public void Intervals_That_Overlap_At_Only_A_Single_EndPoint_Value_That_Is_Included_In_Both_Intervals_Have_A_Single_Value_Intersection_Results()
        {
            var interval1 = Interval.CreateClosedInterval(0, 5);
            var interval2 = Interval.CreateClosedInterval(5, 10);
            
            Assert.IsTrue(interval1.IntersectionWith(interval2).ContainsSinglePoint());
            Assert.IsTrue(interval2.IntersectionWith(interval1).ContainsSinglePoint());
        }

        public void Interval_That_Fully_Contains_Other_Interval_Has_Other_Interval_As_The_Intersection_Result()
        {
            var interval1A = Interval.CreateClosedInterval(5, 10);
            var interval2A = Interval.CreateClosedInterval(0, 20);

            Assert.IsTrue(interval1A.IntersectionWith(interval2A) == interval1A);
            Assert.IsTrue(interval2A.IntersectionWith(interval1A) == interval1A);
        }
    }
}
