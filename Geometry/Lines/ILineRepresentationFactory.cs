using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Lines
{
    public interface ILineRepresentationFactory
    {
        ILineRepresentation CreateLine(PointD point1, PointD point2);
        ILineRepresentation CreateLine(PointD point1, double? slope);
    }
}
