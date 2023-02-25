using Geometry.Intervals;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Geometry.Tests
{
    public class IntervalEndPointTests
    {
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void Two_EndPoints_With_Different_Values_Are_Not_Equal(bool includesPoint1, bool includesPoint2)
        {
            var value1 = 5;
            var value2 = 10;

            var endpoint1 = includesPoint1 ? (IntervalEndPoint)new ClosedIntervalEndPoint(value1) : new OpenIntervalEndPoint(value1);
            var endpoint2 = includesPoint2 ? (IntervalEndPoint)new ClosedIntervalEndPoint(value2) : new OpenIntervalEndPoint(value2);

            AssertInEquality(endpoint1, endpoint2);
        }

        public void Two_EndPoints_With_The_Same_Value_And_The_Same_IncludePoint_Value_Are_Equal()
        {
            var value = 5;

            var endpoint1A = new OpenIntervalEndPoint(value);
            var endpoint2A = new OpenIntervalEndPoint(value);

            AssertEquality(endpoint1A, endpoint2A);

            var endpoint1B = new ClosedIntervalEndPoint(value);
            var endpoint2B = new ClosedIntervalEndPoint(value);

            AssertEquality(endpoint1B, endpoint2B);
        }

        public void Two_EndPoints_With_The_Same_Value_And_Different_IncludePoint_Values_Are_Not_Equal()
        {
            var value = 5;

            var endpoint1 = new OpenIntervalEndPoint(value);
            var endpoint2 = new ClosedIntervalEndPoint(value);

            AssertInEquality<IntervalEndPoint>(endpoint1, endpoint2);
        }

        private void AssertInEquality<T>(T endpoint1, T endpoint2) where T : IntervalEndPoint
        {
            Assert.IsFalse(endpoint1.Equals(endpoint2));
            Assert.IsFalse(endpoint2.Equals(endpoint1));
            Assert.IsFalse(endpoint1 == endpoint2);
            Assert.IsFalse(endpoint2 == endpoint1);

            Assert.IsTrue(endpoint1 != endpoint2);
            Assert.IsTrue(endpoint2 != endpoint1);
        }

        private void AssertEquality<T>(T endpoint1, T endpoint2) where T : IntervalEndPoint
        {
            Assert.IsTrue(endpoint1.Equals(endpoint2));
            Assert.IsTrue(endpoint2.Equals(endpoint1));
            Assert.IsTrue(endpoint1 == endpoint2);
            Assert.IsTrue(endpoint2 == endpoint1);

            Assert.IsFalse(endpoint1 != endpoint2);
            Assert.IsFalse(endpoint2 != endpoint1);
        }
    }
}
