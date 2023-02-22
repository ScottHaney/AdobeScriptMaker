using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Geometry.Lines
{
    public class PointSlopeLineRepresentation : ILineRepresentation, ICanonicalLineFormCapable
    {
        private readonly PointD _point;
        private readonly ISlope _slope;

        public PointSlopeLineRepresentation(PointD point, ISlope slope)
        {
            _point = point;
            _slope = slope;
        }

        public double GetXValue(double yValue)
        {
            return _slope.GetXValue(_point, yValue);
        }

        public double GetYValue(double xValue)
        {
            return _slope.GetYValue(_point, xValue);
        }

        public CanonicalLineForm GetCanonicalLineForm()
        {
            return new CanonicalLineForm(_slope.GetValue(), GetYValue(0));
        }

        public ILineIntersectionResult GetIntersectionWith(ILineRepresentation otherLine)
        {
            if (otherLine is HorizontalLineRepresentation horizontalLineRep)
                return new SinglePointLineIntersectionResult(horizontalLineRep.GetIntersectionPoint(_point, _slope));
            else if (otherLine is VerticalLineRepresentation verticalLineRepresentation)
                return new SinglePointLineIntersectionResult(verticalLineRepresentation.GetIntersectionPoint(_point, _slope));
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
    }
}
