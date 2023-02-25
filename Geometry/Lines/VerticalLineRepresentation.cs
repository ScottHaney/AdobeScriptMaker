using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry.Lines
{
    public class VerticalLineRepresentation : LineRepresentation, IEquatable<VerticalLineRepresentation>
    {
        private readonly double _xValue;

        internal VerticalLineRepresentation(double xValue)
        {
            _xValue = xValue;
        }

        public override double DistanceToPoint(PointD point)
        {
            return Math.Abs(point.X - _xValue);
        }

        public override ParallelBoundingLine[] GetParallelBoundingLines(double distance)
        {
            return new[]
            {
                new ParallelBoundingLine(new VerticalLineRepresentation(_xValue + distance), RelativeLineDirection.AddTo),
                new ParallelBoundingLine(new VerticalLineRepresentation(_xValue - distance), RelativeLineDirection.SubtractedFrom)
            };
        }

        public override ParametricRange GetParametricRange(PointD point1, PointD point2)
        {
            var sorted = new[] { point1, point2 }
                .OrderBy(x => x.Y)
                .ToArray();

            return new ParametricRange(new ParametricPoint(sorted[0], sorted[0].Y), new ParametricPoint(sorted[1], sorted[1].Y));
        }

        public override double GetAngle()
            => 90;

        public override ILineIntersectionResult GetIntersectionWith(LineRepresentation otherLine)
        {
            if (otherLine is HorizontalLineRepresentation horizontalLineRep)
                return new SinglePointLineIntersectionResult(horizontalLineRep.CreateIntersectionPoint(_xValue));
            else if (otherLine is VerticalLineRepresentation verticalLineRepresentation)
            {
                if (_xValue == verticalLineRepresentation._xValue)
                    return new IsSameLineIntersectionResult();
                else
                    return new NoLineIntersectionResult();
            }
            else if (otherLine is TwoPointLineRepresentation twoPointRep)
            {
                var yValue = twoPointRep.GetYValue(_xValue);
                return new SinglePointLineIntersectionResult(new PointD(_xValue, yValue));
            }
            else if (otherLine is PointSlopeLineRepresentation pointSlopeRep)
            {
                var yValue = pointSlopeRep.GetYValue(_xValue);
                return new SinglePointLineIntersectionResult(new PointD(_xValue, yValue));
            }
            else
                throw new NotSupportedException();
        }

        public override bool IsInRange(PointD targetPoint, PointD bound1, PointD bound2)
        {
            var yValues = new[] { bound1.Y, bound2.Y };
            var minY = yValues.Min();
            var maxY = yValues.Max();

            return targetPoint.Y >= minY && targetPoint.Y <= maxY;
        }

        public PointD CreateIntersectionPoint(double yValue)
            => new PointD(_xValue, yValue);

        public PointD GetIntersectionPoint(PointD point, ISlope slope)
            => new PointD(_xValue, slope.GetYValue(point, _xValue));

        public static bool operator ==(VerticalLineRepresentation line1, VerticalLineRepresentation line2)
        {
            if (ReferenceEquals(line1, null))
                return ReferenceEquals(line2, null);

            return line1._xValue == line2._xValue;
        }

        public static bool operator !=(VerticalLineRepresentation line1, VerticalLineRepresentation line2)
            => !(line1 == line2);

        public bool Equals(VerticalLineRepresentation other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return _xValue == other._xValue;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VerticalLineRepresentation);
        }

        public override int GetHashCode()
        {
            return _xValue.GetHashCode();
        }
    }
}