using Geometry.Intervals;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
    }
}
