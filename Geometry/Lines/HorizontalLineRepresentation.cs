using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry.Lines
{
    public class HorizontalLineRepresentation : LineRepresentation, IEquatable<HorizontalLineRepresentation>, IEquatable<LineRepresentation>, ICanonicalLineFormCapable
    {
        private readonly double _yValue;

        internal HorizontalLineRepresentation(double yValue)
        {
            _yValue = yValue;
        }

        public override double DistanceToPoint(PointD point)
        {
            return Math.Abs(point.Y - _yValue);
        }

        public override ParallelBoundingLine[] GetParallelBoundingLines(double distance)
        {
            return new[]
            {
                new ParallelBoundingLine(new HorizontalLineRepresentation(_yValue + distance), RelativeLineDirection.AddTo),
                new ParallelBoundingLine(new HorizontalLineRepresentation(_yValue - distance), RelativeLineDirection.SubtractedFrom)
            };
        }

        public override ParametricRange GetParametricRange(PointD point1, PointD point2)
        {
            var sorted = new[] { point1, point2 }
                .OrderBy(x => x.X)
                .ToArray();

            return new ParametricRange(new ParametricPoint(sorted[0], sorted[0].X), new ParametricPoint(sorted[1], sorted[1].X));
        }

        public override double GetAngle()
            => 0;

        public override ILineIntersectionResult GetIntersectionWith(LineRepresentation otherLine)
        {
            if (otherLine is HorizontalLineRepresentation horizontalLineRep)
            {
                if (_yValue == horizontalLineRep._yValue)
                    return new IsSameLineIntersectionResult();
                else
                    return new NoLineIntersectionResult();
            }
            else if (otherLine is VerticalLineRepresentation verticalLineRepresentation)
                return new SinglePointLineIntersectionResult(verticalLineRepresentation.CreateIntersectionPoint(_yValue));
            else if (otherLine is TwoPointLineRepresentation twoPointRep)
            {
                var xValue = twoPointRep.GetXValue(_yValue);
                return new SinglePointLineIntersectionResult(new PointD(xValue, _yValue));
            }
            else if (otherLine is PointSlopeLineRepresentation pointSlopeRep)
            {
                var xValue = pointSlopeRep.GetXValue(_yValue);
                return new SinglePointLineIntersectionResult(new PointD(xValue, _yValue));
            }
            else
                throw new NotSupportedException();
        }

        public override bool IsInRange(PointD targetPoint, PointD bound1, PointD bound2)
        {
            var xValues = new[] { bound1.X, bound2.X };
            var minX = xValues.Min();
            var maxX = xValues.Max();

            return targetPoint.X >= minX && targetPoint.X <= maxX;
        }

        public PointD CreateIntersectionPoint(double xValue)
            => new PointD(xValue, _yValue);

        public PointD GetIntersectionPoint(PointD point, ISlope slope)
            => new PointD(slope.GetXValue(point, _yValue), _yValue);


        public CanonicalLineForm GetCanonicalLineForm()
        {
            return new CanonicalLineForm(0, _yValue);
        }

        public static bool operator==(HorizontalLineRepresentation line1, HorizontalLineRepresentation line2)
        {
            if (ReferenceEquals(line1, null))
                return ReferenceEquals(line2, null);

            return line1._yValue == line2._yValue;
        }

        public static bool operator!=(HorizontalLineRepresentation line1, HorizontalLineRepresentation line2)
            => !(line1 == line2);

        public bool Equals(HorizontalLineRepresentation other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return _yValue == other._yValue;
        }

        public override bool Equals(LineRepresentation other)
        {
            if (ReferenceEquals(other, null))
                return false;
            else if (ReferenceEquals(other, this))
                return true;

            if (other is ICanonicalLineFormCapable canonicalRep)
                return Equals(canonicalRep);
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as HorizontalLineRepresentation);
        }

        public override int GetHashCode()
        {
            return _yValue.GetHashCode();
        }
    }
}
