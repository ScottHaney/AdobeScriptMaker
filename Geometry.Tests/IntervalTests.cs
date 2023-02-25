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
            //(0, 5) intersect (10, 15) = ()
            TestEmptyIntersectionResult(
                Interval.CreateOpenInterval(0, 5),
                Interval.CreateOpenInterval(10, 15));
        }

        [Test]
        public void Intervals_That_Overlap_At_Only_A_Single_EndPoint_Value_Not_Included_In_Both_Intervals_Have_Empty_Intersection_Results()
        {
            //(0, 5) intersect [5, 10] = ()
            TestEmptyIntersectionResult(
                Interval.CreateOpenInterval(0, 5),
                Interval.CreateClosedInterval(5, 10));

            //[0, 5] intersect (5, 10) = ()
            TestEmptyIntersectionResult(
                Interval.CreateClosedInterval(0, 5),
                Interval.CreateOpenInterval(5, 10));

            //(0, 5) intersect (5, 10) = ()
            TestEmptyIntersectionResult(
                Interval.CreateOpenInterval(0, 5),
                Interval.CreateOpenInterval(5, 10));
        }

        [Test]
        public void Intervals_That_Overlap_At_Only_A_Single_EndPoint_Value_That_Is_Included_In_Both_Intervals_Have_A_Single_Value_Intersection_Result()
        {
            //[0, 5] intersect [5, 10] = [5,5]
            TestSinglePointIntersectionResult(
                Interval.CreateClosedInterval(0, 5),
                Interval.CreateClosedInterval(5, 10));
        }

        [Test]
        public void Interval_That_Fully_Contains_Other_Interval_Has_Other_Interval_As_The_Intersection_Result()
        {
            //[5, 10] intersect [0, 20] = [5, 10]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(5, 10),
                Interval.CreateClosedInterval(0, 20),
                Interval.CreateClosedInterval(5, 10));

            //[5, 10] intersect [5, 20] = [5, 10]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(5, 10),
                Interval.CreateClosedInterval(5, 20),
                Interval.CreateClosedInterval(5, 10));

            //[15, 20] intersect [5, 20] = [15, 20]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(15, 20),
                Interval.CreateClosedInterval(5, 20),
                Interval.CreateClosedInterval(15, 20));

            //(5, 10) intersect (0, 20) = (5, 10)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(5, 10),
                Interval.CreateOpenInterval(0, 20),
                Interval.CreateOpenInterval(5, 10));

            //(5, 10) intersect (5, 20) = (5, 10)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(5, 10),
                Interval.CreateOpenInterval(5, 20),
                Interval.CreateOpenInterval(5, 10));

            //(15, 20) intersect (5, 20) = (15, 20)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(15, 20),
                Interval.CreateOpenInterval(5, 20),
                Interval.CreateOpenInterval(15, 20));

            //[5, 10) intersect (0, 20) = [5, 10)
            TestOverlappingIntervalsIntersectionResult(
                new Interval(new ClosedIntervalEndPoint(5), new OpenIntervalEndPoint(10)),
                new Interval(new OpenIntervalEndPoint(0), new OpenIntervalEndPoint(20)),
                new Interval(new ClosedIntervalEndPoint(5), new OpenIntervalEndPoint(10)));

            //(5, 10] intersect (0, 20) = (5, 10]
            TestOverlappingIntervalsIntersectionResult(
                new Interval(new OpenIntervalEndPoint(5), new ClosedIntervalEndPoint(10)),
                new Interval(new OpenIntervalEndPoint(0), new OpenIntervalEndPoint(20)),
                new Interval(new OpenIntervalEndPoint(5), new ClosedIntervalEndPoint(10)));

            //[5, 10) intersect [0, 20] = [5, 10)
            TestOverlappingIntervalsIntersectionResult(
                new Interval(new ClosedIntervalEndPoint(5), new OpenIntervalEndPoint(10)),
                new Interval(new ClosedIntervalEndPoint(0), new ClosedIntervalEndPoint(20)),
                new Interval(new ClosedIntervalEndPoint(5), new OpenIntervalEndPoint(10)));

            //(5, 10] intersect [0, 20] = (5, 10]
            TestOverlappingIntervalsIntersectionResult(
                new Interval(new OpenIntervalEndPoint(5), new ClosedIntervalEndPoint(10)),
                new Interval(new ClosedIntervalEndPoint(0), new ClosedIntervalEndPoint(20)),
                new Interval(new OpenIntervalEndPoint(5), new ClosedIntervalEndPoint(10)));
        }

        [Test]
        public void Closed_Interval_That_Partially_Contains_Other_Closed_Interval_Has_Partial_Interval_As_The_Intersection_Result()
        {
            //[5, 15] intersect [10, 20] = [10, 15]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(5, 15),
                Interval.CreateClosedInterval(10, 20),
                new Interval(new ClosedIntervalEndPoint(10), new ClosedIntervalEndPoint(15)));

            //[10, 15] intersect [10, 20] = [10, 15]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(10, 15),
                Interval.CreateClosedInterval(10, 20),
                new Interval(new ClosedIntervalEndPoint(10), new ClosedIntervalEndPoint(15)));

            //[12, 15] intersect [10, 20] = [12, 15]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(12, 15),
                Interval.CreateClosedInterval(10, 20),
                new Interval(new ClosedIntervalEndPoint(12), new ClosedIntervalEndPoint(15)));

            //[15, 20] intersect [10, 20] = [15, 20]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(15, 20),
                Interval.CreateClosedInterval(10, 20),
                new Interval(new ClosedIntervalEndPoint(15), new ClosedIntervalEndPoint(20)));

            //[15, 25] intersect [10, 20] = [15, 20]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(15, 25),
                Interval.CreateClosedInterval(10, 20),
                new Interval(new ClosedIntervalEndPoint(15), new ClosedIntervalEndPoint(20)));
        }

        [Test]
        public void Open_Interval_That_Partially_Contains_Other_Open_Interval_Has_Partial_Interval_As_The_Intersection_Result()
        {
            //(5, 15) intersect (10, 20) = (10, 15)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(5, 15),
                Interval.CreateOpenInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(10), new OpenIntervalEndPoint(15)));

            //(10, 15) intersect (10, 20) = (10, 15)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(10, 15),
                Interval.CreateOpenInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(10), new OpenIntervalEndPoint(15)));

            //(12, 15) intersect (10, 20) = (12, 15)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(12, 15),
                Interval.CreateOpenInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(12), new OpenIntervalEndPoint(15)));

            //(15, 20) intersect (10, 20) = (15, 20)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(15, 20),
                Interval.CreateOpenInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(15), new OpenIntervalEndPoint(20)));

            //(15, 25) intersect (10, 20) = (15, 20)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(15, 25),
                Interval.CreateOpenInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(15), new OpenIntervalEndPoint(20)));
        }

        [Test]
        public void Open_Interval_That_Partially_Contains_Other_Closed_Interval_Has_Partial_Interval_As_The_Intersection_Result()
        {
            //(5, 15) intersect [10, 20] = [10, 15)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(5, 15),
                Interval.CreateClosedInterval(10, 20),
                new Interval(new ClosedIntervalEndPoint(10), new OpenIntervalEndPoint(15)));

            //(10, 15) intersect [10, 20] = (10, 15)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(10, 15),
                Interval.CreateClosedInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(10), new OpenIntervalEndPoint(15)));

            //(12, 15) intersect [10, 20] = (12, 15)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(12, 15),
                Interval.CreateClosedInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(12), new OpenIntervalEndPoint(15)));

            //(15, 20) intersect [10, 20] = (15, 20)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(15, 20),
                Interval.CreateClosedInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(15), new OpenIntervalEndPoint(20)));

            //(15, 25) intersect [10, 20] = (15, 20]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateOpenInterval(15, 25),
                Interval.CreateClosedInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(15), new ClosedIntervalEndPoint(20)));
        }

        [Test]
        public void Closed_Interval_That_Partially_Contains_Other_Open_Interval_Has_Partial_Interval_As_The_Intersection_Result()
        {
            //[5, 15] intersect (10, 20) = (10, 15]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(5, 15),
                Interval.CreateOpenInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(10), new ClosedIntervalEndPoint(15)));

            //[10, 15] intersect (10, 20) = (10, 15]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(10, 15),
                Interval.CreateOpenInterval(10, 20),
                new Interval(new OpenIntervalEndPoint(10), new ClosedIntervalEndPoint(15)));

            //[12, 15] intersect (10, 20) = [12, 15]
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(12, 15),
                Interval.CreateOpenInterval(10, 20),
                new Interval(new ClosedIntervalEndPoint(12), new ClosedIntervalEndPoint(15)));

            //[15, 20] intersect (10, 20) = [15, 20)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(15, 20),
                Interval.CreateOpenInterval(10, 20),
                new Interval(new ClosedIntervalEndPoint(15), new OpenIntervalEndPoint(20)));

            //[15, 25] intersect (10, 20) = [15, 20)
            TestOverlappingIntervalsIntersectionResult(
                Interval.CreateClosedInterval(15, 25),
                Interval.CreateOpenInterval(10, 20),
                new Interval(new ClosedIntervalEndPoint(15), new OpenIntervalEndPoint(20)));
        }

        [Test]
        public void Excluding_A_Non_Overlapping_Interval_Returns_The_Original_Interval()
        {
            //[10, 15] exclude [0, 5] = [10, 15]
            TestExclusionResultsForNonOverlappingIntervals(
                Interval.CreateClosedInterval(10, 15),
                Interval.CreateClosedInterval(0, 5));

            //[10, 15] exclude [20, 25] = [10, 15]
            TestExclusionResultsForNonOverlappingIntervals(
                Interval.CreateClosedInterval(10, 15),
                Interval.CreateClosedInterval(20, 25));

            //[10, 15] exclude [0, 10) = [10, 15]
            TestExclusionResultsForNonOverlappingIntervals(
                Interval.CreateClosedInterval(10, 15),
                new Interval(new ClosedIntervalEndPoint(0), new OpenIntervalEndPoint(10)));

            //[10, 15] exclude (15, 25] = [10, 15]
            TestExclusionResultsForNonOverlappingIntervals(
                Interval.CreateClosedInterval(10, 15),
                new Interval(new OpenIntervalEndPoint(15), new ClosedIntervalEndPoint(25)));
        }

        [Test]
        public void Excluding_An_Interval_That_Overlaps_By_A_Single_EndPoint_Works_Correctly()
        {
            //[10, 15] exclude [0, 10] = (10, 15]
            var interval1 = Interval.CreateClosedInterval(10, 15);
            var interval2 = Interval.CreateClosedInterval(0, 10);
            CollectionAssert.AreEqual(new[] { new Interval(new OpenIntervalEndPoint(10), new ClosedIntervalEndPoint(15)) }, interval1.Exclude(interval2).Intervals);

            //[10, 15] exclude [15, 20] = [10, 15)
            var interval1B = Interval.CreateClosedInterval(10, 15);
            var interval2B = Interval.CreateClosedInterval(15, 20);
            CollectionAssert.AreEqual(new[] { new Interval(new ClosedIntervalEndPoint(10), new OpenIntervalEndPoint(15)) }, interval1B.Exclude(interval2B).Intervals);
        }



        private void TestExclusionResultsForNonOverlappingIntervals(Interval interval1, Interval interval2)
        {
            var exclusionResult = interval1.Exclude(interval2);
            CollectionAssert.AreEqual(new[] { interval1 }, exclusionResult.Intervals);
        }

        private void TestSinglePointIntersectionResult(Interval interval1, Interval interval2)
        {
            Assert.IsTrue(interval1.IntersectionWith(interval2).ContainsSinglePoint());
            Assert.IsTrue(interval2.IntersectionWith(interval1).ContainsSinglePoint());
        }

        private void TestEmptyIntersectionResult(Interval interval1, Interval interval2)
        {
            Assert.IsTrue(interval1.IntersectionWith(interval2).IsEmpty());
            Assert.IsTrue(interval2.IntersectionWith(interval1).IsEmpty());
        }

        private void TestOverlappingIntervalsIntersectionResult(Interval interval1,
            Interval interval2,
            Interval expectedResult)
        {
            var result1 = interval1.IntersectionWith(interval2);
            var result2 = interval2.IntersectionWith(interval1);

            Assert.IsTrue(result1 == expectedResult);
            Assert.IsTrue(result2 == expectedResult);
            Assert.IsFalse(result1 != expectedResult);
            Assert.IsFalse(result2 != expectedResult);

            Assert.AreEqual(expectedResult, result1);
            Assert.AreEqual(expectedResult, result2);
        }
    }
}
