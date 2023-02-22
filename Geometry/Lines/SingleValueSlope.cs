﻿using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Geometry.Lines
{
    public class SingleValueSlope : ISlope
    {
        private readonly double _value;

        public SingleValueSlope(double value)
        {
            _value = value;
        }

        public double GetXValue(PointD startPoint, double targetY)
        {
            var requiredRise = targetY - startPoint.Y;
            var run = requiredRise / _value;

            return startPoint.X + run;
        }

        public double GetYValue(PointD startPoint, double targetX)
        {
            var requiredRun = targetX - startPoint.X;
            var rise = _value * requiredRun;

            return startPoint.Y + rise;
        }

        public double GetValue()
            => _value;
    }
}
