using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Geometry.Lines
{
    public class LineSegment
    {
        private readonly ILineRepresentation _line;
        private readonly PointD[] _bounds;

        public LineSegment(PointD point1, PointD point2)
        {
            _line = new TwoPointLineRepresentation(point1, point2);
            _bounds = new[] { point1, point2 };
        }

        public bool IntersectsWith(LineSegment other)
        {
            var intersectionResult = _line.GetIntersectionWith(other._line);
            if (intersectionResult.HasNoPointsInCommonWith)
                return false;
            else if (intersectionResult.IsTheSameLine)
                throw new NotImplementedException();
            else
            {
                var intersectionPoint = intersectionResult.GetStart();
                var touchesThisLineSegment = _line.IsInRange(intersectionPoint, _bounds[0], _bounds[1]);
                var touchesOtherLineSegment = other._line.IsInRange(intersectionPoint, other._bounds[0], other._bounds[1]);

                return touchesThisLineSegment && touchesOtherLineSegment;
            }
        }

        public ILineRepresentation ToLine()
        {
            return _line;
        }
    }
}
