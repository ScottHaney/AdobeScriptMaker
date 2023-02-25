using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using Geometry.Intervals;
using Geometry.Lines;

namespace Geometry.LineSegments
{
    public class LineSegment : IEquatable<LineSegment>
    {
        private readonly LineRepresentation _line;
        private readonly PointD[] _bounds;

        public PointD StartPoint => _bounds[0];
        public PointD EndPoint => _bounds[1];

        private static readonly ILineRepresentationFactory _factory = new LineRepresentationFactory();
        private static readonly ILineSegmentRepresentationFactory _segmentFactory = new LineSegmentRepresentationFactory(_factory);

        internal LineSegment(LineRepresentation line, params PointD[] bounds)
        {
            _line = line;
            _bounds = bounds;
        }

        public LineSegment ShiftY(double yShift)
        {
            return _segmentFactory.Create(new PointD(StartPoint.X, StartPoint.Y + yShift), new PointD(EndPoint.X, EndPoint.Y + yShift));
        }

        public static IEnumerable<LineSegment> Join(params LineSegment[] segments)
        {
            var groups = segments
                .GroupBy(x => x._line)
                .ToList();

            foreach (var group in groups)
            {
                var items = group.ToList();
                if (items.Count == 1)
                    yield return items.First();
                else
                {
                    var pointsMap = new Dictionary<double, PointD>();

                    var intervals = items
                        .Select(x => x.ToInterval(pointsMap))
                        .OrderBy(x => x.Start.Value)
                        .ToArray();

                    var currentInterval = intervals.First();
                    for (int i = 1; i < intervals.Length; i++)
                    {
                        var nextInterval = intervals[i];
                        if (currentInterval.TryConnectWith(nextInterval, out var combinedInterval))
                            currentInterval = combinedInterval;
                        else
                        {
                            yield return _segmentFactory.Create(pointsMap[currentInterval.Start.Value], pointsMap[currentInterval.End.Value]);
                            currentInterval = nextInterval;
                        }
                    }

                    yield return _segmentFactory.Create(pointsMap[currentInterval.Start.Value], pointsMap[currentInterval.End.Value]);
                }
            }
        }

        public IEnumerable<LineSegment> Exclude(params LineSegment[] otherSegments)
        {
            var pointsMap = new Dictionary<double, PointD>();
            var currentRange = ToInterval(pointsMap);

            var otherRanges = otherSegments
                .Where(x => _line == x._line)
                .Select(x => x.ToInterval(pointsMap)).ToArray();

            var finalResult = currentRange.Exclude(otherRanges);
            foreach (var item in finalResult.PositiveLengthIntervals)
            {
                yield return new LineSegment(_line, pointsMap[item.Start.Value], pointsMap[item.End.Value]);
            }
        }

        private Interval ToInterval(Dictionary<double, PointD> pointsMap = null)
        {
            var range = GetParametricRange();

            if (pointsMap != null)
            {
                pointsMap[range.Start.ParametricValue] = range.Start.Point;
                pointsMap[range.End.ParametricValue] = range.End.Point;
            }

            return Interval.CreateClosedInterval(range.Start.ParametricValue, range.End.ParametricValue);
        }

        public double GetLength()
        {
            return Math.Sqrt(Math.Pow(_bounds[0].X - _bounds[1].X, 2) + Math.Pow(_bounds[0].Y - _bounds[1].Y, 2));
        }

        public ParametricRange GetParametricRange()
        {
            return _line.GetParametricRange(_bounds[0], _bounds[1]);
        }

        public bool TryJoinWith(LineSegment other, out PointD[] matchingPoints)
        {
            matchingPoints = _bounds.Where(x => other._bounds.Contains(x)).ToArray();
            return matchingPoints.Length > 0;
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
                return PointIsInRangeOfBothSegments(intersectionPoint.Value, this, other);
            }
        }

        public bool OverlapsWith(LineSegment other)
        {
            if (_line == other._line)
            {
                var range1 = ToInterval();
                var range2 = other.ToInterval();

                return range1.IntersectionWith(range2).ContainsMoreThanOnePoint();
            }
            else
                return false;
        }

        private bool PointIsInRangeOfBothSegments(PointD point, LineSegment segment1, LineSegment segment2)
        {
            var touchesThisLineSegment = segment1._line.IsInRange(point, segment1._bounds[0], segment1._bounds[1]);
            var touchesOtherLineSegment = segment2._line.IsInRange(point, segment2._bounds[0], segment2._bounds[1]);

            return touchesThisLineSegment && touchesOtherLineSegment;
        }

        public LineRepresentation ToLine()
        {
            return _line;
        }

        public Line ToLegacyLine()
        {
            return new Line(new PointF((float)_bounds[0].X, (float)_bounds[0].Y), new PointF((float)_bounds[1].X, (float)_bounds[1].Y));
        }

        public static bool operator ==(LineSegment lineSegment1, LineSegment lineSegment2)
        {
            if (ReferenceEquals(lineSegment1, null))
                return ReferenceEquals(lineSegment2, null);

            return lineSegment1.Equals(lineSegment2);
        }

        public static bool operator !=(LineSegment lineSegment1, LineSegment lineSegment2)
            => !(lineSegment1 == lineSegment2);

        public bool Equals(LineSegment other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (StartPoint == other.StartPoint && EndPoint == other.EndPoint)
                return true;
            else if (EndPoint == other.StartPoint && StartPoint == other.EndPoint)
                return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LineSegment);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = 0;
                foreach (var bound in _bounds)
                    result += bound.GetHashCode();

                return result;
            }
        }

        public override string ToString()
        {
            return $"[({_bounds[0].X}, {_bounds[0].Y}), ({_bounds[1].X}, {_bounds[1].Y})]";
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
