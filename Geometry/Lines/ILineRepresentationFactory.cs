using System;
using System.Collections.Generic;
using System.Text;
using Geometry.LineSegments;

namespace Geometry.Lines
{
    public interface ILineRepresentationFactory
    {
        LineRepresentation CreateLine(PointD point1, PointD point2);
        LineRepresentation CreateLine(PointD point1, double? slope);
    }
}
