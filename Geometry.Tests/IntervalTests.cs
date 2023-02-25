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
    }
}
