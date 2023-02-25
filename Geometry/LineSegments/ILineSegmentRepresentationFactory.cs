using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.LineSegments
{
    public interface ILineSegmentRepresentationFactory
    {
        LineSegment Create(PointD point1, PointD point2);
    }
}
