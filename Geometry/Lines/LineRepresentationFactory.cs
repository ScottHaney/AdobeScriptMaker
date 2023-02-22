using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Lines
{
    public class LineRepresentationFactory : ILineRepresentationFactory
    {
        public ILineRepresentation CreateLine(PointD point1, PointD point2)
        {
            if (point1.Y == point2.Y)
                return new HorizontalLineRepresentation(point1.Y);
            else if (point1.X == point2.X)
                return new VerticalLineRepresentation(point1.X);
            else
                return new TwoPointLineRepresentation(point1, point2);
        }

        public ILineRepresentation CreateLine(PointD point1, double? slope)
        {
            if (slope == null)
                return new VerticalLineRepresentation(point1.X);
            else if (slope == 0)
                return new HorizontalLineRepresentation(point1.Y);
            else
                return new PointSlopeLineRepresentation(point1, new SingleValueSlope(slope.Value));
        }
    }
}
