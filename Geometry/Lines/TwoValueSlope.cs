using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Lines
{
    public class TwoValueSlope : ISlope
    {
        private readonly double _rise;
        private readonly double _run;

        public TwoValueSlope(double rise, double run)
        {
            _rise = rise;
            _run = run;
        }

        public TwoValueSlope(PointD point1, PointD point2)
            : this(point1.Y - point2.Y, point1.X - point2.X)
        { }

        public double GetXValue(PointD startPoint, double targetY)
        {
            var requiredRise = targetY - startPoint.Y;
            var run = requiredRise / (_rise / _run);

            return startPoint.X + run;
        }

        public double GetYValue(PointD startPoint, double targetX)
        {
            var requiredRun = targetX - startPoint.X;
            var rise = (_rise / _run) * requiredRun;

            return startPoint.Y + rise;
        }

        public double GetValue()
            => _rise / _run;
    }
}
