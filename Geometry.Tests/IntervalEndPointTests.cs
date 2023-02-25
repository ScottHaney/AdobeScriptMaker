using Geometry.Intervals;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

            Assert.AreNotEqual(endpoint1, endpoint2);
            Assert.AreNotEqual(endpoint2, endpoint1);
        }

        public void Two_EndPoints_With_The_Same_Value_And_The_Same_IncludePoint_Value_Are_Equal()
        {
            var value = 5;

            var endpoint1A = new OpenIntervalEndPoint(value);
            var endpoint2A = new OpenIntervalEndPoint(value);

            Assert.AreEqual(endpoint1A, endpoint2A);

            var endpoint1B = new ClosedIntervalEndPoint(value);
            var endpoint2B = new ClosedIntervalEndPoint(value);

            Assert.AreEqual(endpoint1B, endpoint2B);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Assertion", "NUnit2021:Incompatible types for EqualTo constraint", Justification = "Ensure that no IEquatable implementation slips in to break the constraint that the two end point types can never be equal")]
        public void Two_EndPoints_With_The_Same_Value_And_Different_IncludePoint_Values_Are_Not_Equal()
        {
            var value = 5;

            var endpoint1 = new OpenIntervalEndPoint(value);
            var endpoint2 = new ClosedIntervalEndPoint(value);

            Assert.AreNotEqual(endpoint1, endpoint2);
            Assert.AreNotEqual(endpoint2, endpoint1);
        }
    }
}
