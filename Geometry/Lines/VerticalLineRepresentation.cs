using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry.Lines
{
    public class VerticalLineRepresentation : ILineRepresentation
    {
        private readonly double _xValue;

        public VerticalLineRepresentation(double xValue)
        {
            _xValue = xValue;
        }

        public ParametricRange GetParametricRange(PointD point1, PointD point2)
        {
            var sorted = new[] { point1, point2 }
                .OrderBy(x => x.Y)
                .ToArray();

            return new ParametricRange(new ParametricPoint(sorted[0], sorted[0].Y), new ParametricPoint(sorted[1], sorted[1].Y));
        }

        public ILineIntersectionResult GetIntersectionWith(ILineRepresentation otherLine)
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

        public bool IsInRange(PointD targetPoint, PointD bound1, PointD bound2)
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
    }
}