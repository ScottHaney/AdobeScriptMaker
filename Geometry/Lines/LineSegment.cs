using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public LineSegment(ILineRepresentation line, params PointD[] bounds)
        {
            _line = line;
            _bounds = bounds;
        }

        public ParametricRange GetParametricRange()
        {
            return _line.GetParametricRange(_bounds[0], _bounds[1]);
        }

        public bool IntersectsWith(LineSegment other)
        {
            var intersectionResult = _line.GetIntersectionWith(other._line);
            if (intersectionResult.HasNoPointsInCommonWith)
                return false;
            else if (intersectionResult.IsTheSameLine)
            {
                return _bounds.Concat(other._bounds).Any(x => PointIsInRangeOfBothSegments(x, this, other));
            }
            else
            {
                var intersectionPoint = intersectionResult.GetStart();
                return PointIsInRangeOfBothSegments(intersectionPoint, this, other);
            }
        }

        private bool PointIsInRangeOfBothSegments(PointD point, LineSegment segment1, LineSegment segment2)
        {
            var touchesThisLineSegment = segment1._line.IsInRange(point, segment1._bounds[0], segment1._bounds[1]);
            var touchesOtherLineSegment = segment2._line.IsInRange(point, segment2._bounds[0], segment2._bounds[1]);

            return touchesThisLineSegment && touchesOtherLineSegment;
        }

        public ILineRepresentation ToLine()
        {
            return _line;
        }
    }
}
