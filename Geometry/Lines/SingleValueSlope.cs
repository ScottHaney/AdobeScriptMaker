using System;
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

        public ArcLengthInfo GetDistanceInfoForArcLength(PointD startPoint, double arcLength)
        {
            var angle = GetAngle();
            return new ArcLengthInfo(arcLength * Math.Cos(angle), arcLength * Math.Sin(angle));
        }

        public double GetAngle()
        {
            return Math.Atan(_value);
            var value = Math.Atan(_value);
            if (value < 0)
                value += Math.PI;

            return value;
        }

        public ISlope GetPerpendicularSlope()
        {
            return new SingleValueSlope(-1 / _value);
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
