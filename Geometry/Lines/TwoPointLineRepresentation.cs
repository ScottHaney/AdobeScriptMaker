using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Geometry.Lines
{
    public class TwoPointLineRepresentation : ILineRepresentation, ICanonicalLineFormCapable, IEquatable<ICanonicalLineFormCapable>
    {
        private readonly PointD _point1;
        private readonly PointD _point2;
        private readonly ISlope _slope;

        internal TwoPointLineRepresentation(PointD point1, PointD point2)
        {
            _point1 = point1;
            _point2 = point2;

            _slope = new TwoValueSlope(point1, point2);
        }

        public ParallelBoundingLine[] GetParallelBoundingLines(double distance)
        {
            var perpendicularSlope = _slope.GetPerpendicularSlope();
            var diffInfoP1 = perpendicularSlope.GetDistanceInfoForArcLength(_point1, distance);
            var diffInfoP2 = perpendicularSlope.GetDistanceInfoForArcLength(_point2, distance);

            return new[]
            {
                new ParallelBoundingLine(
                    new TwoPointLineRepresentation(
                        new PointD(_point1.X + diffInfoP1.XDiff, _point1.Y + diffInfoP1.YDiff),
                        new PointD(_point2.X + diffInfoP2.XDiff, _point2.Y + diffInfoP2.YDiff)),
                    RelativeLineDirection.GreaterThan),
                new ParallelBoundingLine(
                    new TwoPointLineRepresentation(
                        new PointD(_point1.X - diffInfoP1.XDiff, _point1.Y - diffInfoP1.YDiff),
                        new PointD(_point2.X - diffInfoP2.XDiff, _point2.Y - diffInfoP2.YDiff)),
                    RelativeLineDirection.LessThan)
            };
        }

        public ParametricRange GetParametricRange(PointD point1, PointD point2)
        {
            var sorted = new[] { point1, point2 }
                .OrderBy(x => x.X)
                .ToArray();

            return new ParametricRange(new ParametricPoint(sorted[0], sorted[0].X), new ParametricPoint(sorted[1], sorted[1].X));
        }

        public double GetXValue(double yValue)
            => _slope.GetXValue(_point1, yValue);

        public double GetYValue(double xValue)
            => _slope.GetYValue(_point1, xValue);

        public CanonicalLineForm GetCanonicalLineForm()
        {
            return new CanonicalLineForm(_slope.GetValue(), GetYValue(0));
        }

        public ILineIntersectionResult GetIntersectionWith(ILineRepresentation otherLine)
        {
            if (otherLine is HorizontalLineRepresentation horizontalLineRep)
                return new SinglePointLineIntersectionResult(horizontalLineRep.GetIntersectionPoint(_point1, _slope));
            else if (otherLine is VerticalLineRepresentation verticalLineRepresentation)
                return new SinglePointLineIntersectionResult(verticalLineRepresentation.GetIntersectionPoint(_point1, _slope));
            else if (otherLine is ICanonicalLineFormCapable lineRep)
            {
                var thisLineCanonicalForm = GetCanonicalLineForm();
                var otherLineCanonicalForm = lineRep.GetCanonicalLineForm();

                var xValue = (thisLineCanonicalForm.YIntercept - otherLineCanonicalForm.YIntercept) / (thisLineCanonicalForm.Slope - otherLineCanonicalForm.Slope);
                var yValue = thisLineCanonicalForm.Slope * xValue + thisLineCanonicalForm.YIntercept;

                return new SinglePointLineIntersectionResult(new PointD(xValue, yValue));
            }
            else
                throw new NotSupportedException();
        }

        public bool IsInRange(PointD targetPoint, PointD bound1, PointD bound2)
        {
            var xValues = new[] { bound1.X, bound2.X };
            var minX = xValues.Min();
            var maxX = xValues.Max();

            return targetPoint.X >= minX && targetPoint.X <= maxX;
        }

        public bool Equals(ICanonicalLineFormCapable other)
        {
            if (ReferenceEquals(other, null))
                return false;
            
            return GetCanonicalLineForm() == other.GetCanonicalLineForm();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ICanonicalLineFormCapable);
        }

        public override int GetHashCode()
        {
            return GetCanonicalLineForm().GetHashCode();
        }
    }
}
