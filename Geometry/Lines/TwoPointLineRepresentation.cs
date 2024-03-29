﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Geometry.Lines
{
    public class TwoPointLineRepresentation : LineRepresentation, ICanonicalLineFormCapable, IEquatable<ICanonicalLineFormCapable>, IEquatable<LineRepresentation>
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

        public override double DistanceToPoint(PointD point)
        {
            //Taken from wikipedia
            var numerator = Math.Abs((_point2.X - _point1.X) * (_point1.Y - point.Y) - (_point1.X - point.X) * (_point2.Y - _point1.Y));
            var denominator = Math.Sqrt(Math.Pow(_point2.X - _point1.X, 2) + Math.Pow(_point2.Y - _point1.Y, 2));

            return numerator / denominator;
        }

        public override ParallelBoundingLine[] GetParallelBoundingLines(double distance)
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
                    RelativeLineDirection.AddTo),
                new ParallelBoundingLine(
                    new TwoPointLineRepresentation(
                        new PointD(_point1.X - diffInfoP1.XDiff, _point1.Y - diffInfoP1.YDiff),
                        new PointD(_point2.X - diffInfoP2.XDiff, _point2.Y - diffInfoP2.YDiff)),
                    RelativeLineDirection.SubtractedFrom)
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
            => _slope.GetAngle();

        public double GetXValue(double yValue)
            => _slope.GetXValue(_point1, yValue);

        public double GetYValue(double xValue)
            => _slope.GetYValue(_point1, xValue);

        public CanonicalLineForm GetCanonicalLineForm()
        {
            return new CanonicalLineForm(_slope.GetValue(), GetYValue(0));
        }

        public override ILineIntersectionResult GetIntersectionWith(LineRepresentation otherLine)
        {
            if (otherLine is HorizontalLineRepresentation horizontalLineRep)
                return new SinglePointLineIntersectionResult(horizontalLineRep.GetIntersectionPoint(_point1, _slope));
            else if (otherLine is VerticalLineRepresentation verticalLineRepresentation)
                return new SinglePointLineIntersectionResult(verticalLineRepresentation.GetIntersectionPoint(_point1, _slope));
            else if (otherLine is ICanonicalLineFormCapable lineRep)
            {
                var thisLineCanonicalForm = GetCanonicalLineForm();
                var otherLineCanonicalForm = lineRep.GetCanonicalLineForm();

                var xValue = (thisLineCanonicalForm.YIntercept - otherLineCanonicalForm.YIntercept) / (otherLineCanonicalForm.Slope - thisLineCanonicalForm.Slope);
                var yValue = thisLineCanonicalForm.Slope * xValue + thisLineCanonicalForm.YIntercept;

                return new SinglePointLineIntersectionResult(new PointD(xValue, yValue));
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

        public bool Equals(ICanonicalLineFormCapable other)
        {
            if (ReferenceEquals(other, null))
                return false;
            else if (ReferenceEquals(other, this))
                return true;

            return GetCanonicalLineForm() == other.GetCanonicalLineForm();
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
            return Equals(obj as ICanonicalLineFormCapable);
        }

        public override int GetHashCode()
        {
            return GetCanonicalLineForm().GetHashCode();
        }
    }
}
