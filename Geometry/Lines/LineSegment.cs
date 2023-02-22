using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
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

        public LineSegment(PointF point1, PointF point2)
            : this(new PointD(point1), new PointD(point2))
        { }

        public LineSegment(ILineRepresentation line, params PointD[] bounds)
        {
            _line = line;
            _bounds = bounds;
        }

        public ParametricRange GetParametricRange()
        {
            return _line.GetParametricRange(_bounds[0], _bounds[1]);
        }

        public bool TryConnectWith(LineSegment other, out LineSegment combinedSegment)
        {
            var intersectionResult = ToLine().GetIntersectionWith(other.ToLine());
            if (intersectionResult.IsTheSameLine)
            {
                var thisParametricRange = GetParametricRange();
                var otherParametricRange = other.GetParametricRange();

                if (thisParametricRange.TryConnectOtherRange(otherParametricRange, out var combinedRange))
                {
                    combinedSegment = new LineSegment(_line, combinedRange.Start.Point, combinedRange.End.Point);
                    return true;
                }
            }

            combinedSegment = null;
            return false;
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

        public Line ToLegacyLine()
        {
            return new Line(new PointF((float)_bounds[0].X, (float)_bounds[0].Y), new PointF((float)_bounds[1].X, (float)_bounds[1].Y));
        }
    }

    public class LineSegmentLineEqualityComparer : IEqualityComparer<LineSegment>
    {
        public bool Equals([AllowNull] LineSegment x, [AllowNull] LineSegment y)
        {
            if (ReferenceEquals(x, null))
                return ReferenceEquals(y, null);

            return x.ToLine().Equals(y.ToLine());
        }

        public int GetHashCode([DisallowNull] LineSegment obj)
        {
            return obj?.ToLine().GetHashCode() ?? 19;
        }
    }
}
