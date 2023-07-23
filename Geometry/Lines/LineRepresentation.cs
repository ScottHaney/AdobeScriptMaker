using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Geometry.Lines
{
    public abstract class LineRepresentation : IEquatable<LineRepresentation>
    {
        public abstract ILineIntersectionResult GetIntersectionWith(LineRepresentation otherLine);
        public abstract bool IsInRange(PointD targetPoint, PointD bound1, PointD bound2);

        public abstract ParametricRange GetParametricRange(PointD point1, PointD point2);
        public abstract ParallelBoundingLine[] GetParallelBoundingLines(double distance);
        public abstract double DistanceToPoint(PointD point);
        public abstract double GetAngle();

        public static bool operator==(LineRepresentation left, LineRepresentation right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);
        }

        public static bool operator !=(LineRepresentation left, LineRepresentation right)
            => !(left == right);

        public abstract bool Equals(LineRepresentation other);

        public override bool Equals(object obj)
        {
            return Equals(obj as LineRepresentation);
        }
    }

    public class ParallelBoundingLine
    {
        public readonly LineRepresentation Line;
        public readonly RelativeLineDirection Direction;

        public ParallelBoundingLine(LineRepresentation line, RelativeLineDirection direction)
        {
            Line = line;
            Direction = direction;
        }
    }

    public enum RelativeLineDirection
    {
        AddTo,
        SubtractedFrom
    }

    public class IsTheSameLineEqualityComparer : IEqualityComparer<LineRepresentation>
    {
        public bool Equals(LineRepresentation x, LineRepresentation y)
        {
            if (ReferenceEquals(x, null))
                return ReferenceEquals(y, null);

            var intersectionResult = x.GetIntersectionWith(y);
            return intersectionResult.IsTheSameLine;
        }

        public int GetHashCode(LineRepresentation obj)
        {
            return obj?.GetHashCode() ?? 19;
        }
    }

    public class ParametricRange
    {
        public readonly ParametricPoint Start;
        public readonly ParametricPoint End;

        public ParametricRange(ParametricPoint start, ParametricPoint end)
        {
            Start = start;
            End = end;
        }

        public bool OverlapsOrConnectsWith(ParametricRange other)
        {
            if (other.Start.ParametricValue >= Start.ParametricValue && other.Start.ParametricValue <= End.ParametricValue)
                return true;
            else if (other.End.ParametricValue <= End.ParametricValue && other.End.ParametricValue >= Start.ParametricValue)
                return true;
            else
                return false;
        }

        public bool OverlapsWith(ParametricRange other)
        {
            if (other.Start.ParametricValue > Start.ParametricValue && other.Start.ParametricValue < End.ParametricValue)
                return true;
            else if (other.End.ParametricValue < End.ParametricValue && other.End.ParametricValue > Start.ParametricValue)
                return true;
            else if (other.Start.ParametricValue == Start.ParametricValue && other.End.ParametricValue == End.ParametricValue)
                return true;
            else
                return false;
        }

        public bool TryConnectOtherRange(ParametricRange other, out ParametricRange combinedRange)
        {
            if (other.Start.ParametricValue == Start.ParametricValue && other.End.ParametricValue == End.ParametricValue)
            {
                combinedRange = new ParametricRange(Start, End);
                return true;
            }
            else if (other.Start.ParametricValue == End.ParametricValue)
            {
                combinedRange = new ParametricRange(Start, other.End);
                return true;
            }
            else if (other.End.ParametricValue == Start.ParametricValue)
            {
                combinedRange = new ParametricRange(other.Start, End);
                return true;
            }
            else
            {
                combinedRange = null;
                return false;
            }
        }
    }

    public class ParametricPoint
    {
        public readonly PointD Point;
        public readonly double ParametricValue;

        public ParametricPoint(PointD point,
            double parametricValue)
        {
            Point = point;
            ParametricValue = parametricValue;
        }
    }

    public interface ICanonicalLineFormCapable
    {
        CanonicalLineForm GetCanonicalLineForm();
    }

    public interface ILineIntersectionResult
    {
        bool HasSinglePointIntersection { get; }
        bool HasNoPointsInCommonWith { get; }
        bool IsTheSameLine { get; }
        PointD? GetStart();
        PointD? GetEnd();
    }

    public class SinglePointLineIntersectionResult : ILineIntersectionResult
    {
        private readonly PointD _intersectionPoint;

        public bool HasSinglePointIntersection => true;

        public bool HasNoPointsInCommonWith => false;

        public bool IsTheSameLine => false;

        public SinglePointLineIntersectionResult(PointD intersectionPoint)
        {
            _intersectionPoint = intersectionPoint;
        }

        public PointD? GetStart()
            => _intersectionPoint;

        public PointD? GetEnd()
            => _intersectionPoint;
    }

    public class NoLineIntersectionResult : ILineIntersectionResult
    {
        public bool HasSinglePointIntersection => false;
        public bool HasNoPointsInCommonWith => true;
        public bool IsTheSameLine => false;

        public PointD? GetStart()
            => null;

        public PointD? GetEnd()
            => null;
    }

    public class IsSameLineIntersectionResult : ILineIntersectionResult
    {
        public bool HasSinglePointIntersection => false;
        public bool HasNoPointsInCommonWith => false;
        public bool IsTheSameLine => true;

        public PointD? GetStart()
            => null;

        public PointD? GetEnd()
            => null;
    }
}
