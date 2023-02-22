using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Lines
{
    public class Slope : IEquatable<Slope>
    {
        private readonly double? _slope;

        public bool IsVerticalLine => _slope == null;
        public bool IsHorizontalLine => _slope == 0;

        public Slope(PointD point1, PointD point2)
        {
            var rise = point1.Y - point2.Y;
            var run = point1.X - point2.X;

            if (run == 0)
                _slope = null;
            else
                _slope = rise / run;
        }

        public Slope(double? slope)
        {
            _slope = slope;
        }

        public double GetXValue(PointD startPoint, double targetY)
        {
            var requiredRise = targetY - startPoint.Y;
            var run = requiredRise / _slope.Value;

            return startPoint.X + run;
        }

        public double GetYValue(PointD startPoint, double targetX)
        {
            var requiredRun = targetX - startPoint.X;
            var rise = _slope.Value * requiredRun;

            return startPoint.Y + rise;
        }

        public Slope GetPerpendicularSlope()
        {
            if (IsVerticalLine)
                return new Slope(0);
            else
                return new Slope(-1 / _slope.Value);
        }

        public static bool operator ==(Slope a, Slope b)
        {
            if (ReferenceEquals(a, null))
                return ReferenceEquals(b, null);

            return a.Equals(b);
        }

        public static bool operator !=(Slope a, Slope b)
            => !(a == b);

        public bool Equals(Slope other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return _slope == other._slope;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Slope);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _slope?.GetHashCode() ?? 19;
            }
        }
    }
}
