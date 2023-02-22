using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geometry.Lines
{
    public class HorizontalLineRepresentation : ILineRepresentation
    {
        private readonly double _yValue;

        public HorizontalLineRepresentation(double yValue)
        {
            _yValue = yValue;
        }

        public ILineIntersectionResult GetIntersectionWith(ILineRepresentation otherLine)
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

        public bool IsInRange(PointD targetPoint, PointD bound1, PointD bound2)
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
    }
}
