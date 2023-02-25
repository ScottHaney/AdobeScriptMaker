using Geometry.Lines;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;

namespace Geometry.Intervals
{
    public class Interval
    {
        private readonly IntervalEndPoint _start;
        private readonly IntervalEndPoint _end;

        public Interval(IntervalEndPoint startPoint, IntervalEndPoint endPoint)
        {
            if (startPoint.Value > endPoint.Value)
                throw new InvalidIntervalException(startPoint, endPoint);

            _start = startPoint;
            _end = endPoint;
        }

        public static Interval CreateClosedInterval(double start, double end)
            => new Interval(new ClosedIntervalEndPoint(start), new ClosedIntervalEndPoint(end));

        public static Interval CreateOpenInterval(double start, double end)
            => new Interval(new OpenIntervalEndPoint(start), new OpenIntervalEndPoint(end));

        public static readonly Interval Empty = Interval.CreateOpenInterval(0, 0);

        public Interval IntersectionWith(Interval otherInterval)
        {
            //Move the current interval from left to right starting across the otherInterval starting with the current
            //interval being fully to the left of the otherInterval and ending with the current interval being
            //fully to the right of the otherInterval

            if (_end.Value < otherInterval._start.Value)
                return Empty;
            else if (_end.Value == otherInterval._start.Value)
                return new Interval(_end, otherInterval._start);
            else if (_end.Value <= otherInterval._end.Value)
            {
                IntervalEndPoint endPointToUse;
                if (_end.Value < otherInterval._end.Value)
                    endPointToUse = _end;
                else
                    endPointToUse = OverlapIdenticallyValuedPoints(_end, otherInterval._end);

                if (_start.Value < otherInterval._start.Value)
                    return new Interval(otherInterval._start, endPointToUse);
                else if (_start.Value == otherInterval._start.Value)
                    return new Interval(OverlapIdenticallyValuedPoints(_start, otherInterval._start), endPointToUse);
                else
                    return new Interval(_start, endPointToUse);
            }
            else
            {
                if (_start.Value < otherInterval._end.Value)
                    return new Interval(_start, otherInterval._end);
                else if (_start.Value == otherInterval._end.Value)
                    return new Interval(_start, otherInterval._end);
                else
                    return Empty;
            }
        }

        private IntervalEndPoint OverlapIdenticallyValuedPoints(IntervalEndPoint p1, IntervalEndPoint p2)
        {
            if (p1.Value != p2.Value)
                throw new InvalidOperationException("Both points must have the same value to run this operation!");

            if (p1.IncludesPoint && p2.IncludesPoint)
                return p1;
            else
                return new OpenIntervalEndPoint(p1.Value);
        }

        public bool IsEmpty()
        {
            if (_end.Value > _start.Value)
                return false;
            else
                return !IncludesValue(_end.Value);
        }

        public bool ContainsSinglePoint()
        {
            if (_end.Value != _start.Value)
                return false;
            else
                return _end.IncludesPoint && _start.IncludesPoint;
        }

        private bool IncludesValue(double value)
        {
            if (value > _start.Value && value < _end.Value)
                return true;
            else if (value < _start.Value || value > _end.Value)
                return false;
            else
            {
                //Make sure that each end point that matches the value (could be both end points in an interval like [9,9]) includes the point
                return new[] { _start, _end }
                    .Where(x => x.Value == value)
                    .All(x => x.IncludesPoint);
            }
        }

        public static implicit operator IntervalsIntersectionResult(Interval interval) => new IntervalsIntersectionResult(interval);
    }

    public class IntervalsIntersectionResult
    {
        private readonly Interval[] _resultingIntervals;

        public static readonly IntervalsIntersectionResult Empty = new IntervalsIntersectionResult();

        public bool IsEmpty => _resultingIntervals.Length == 0 || _resultingIntervals.All(x => x.IsEmpty());

        public IEnumerable<Interval> Intervals => _resultingIntervals.Where(x => !x.IsEmpty());

        public IntervalsIntersectionResult(params Interval[] resultingIntervals)
        {
            _resultingIntervals = resultingIntervals ?? Array.Empty<Interval>();
        }
    }

    public abstract class IntervalEndPoint : IEquatable<IntervalEndPoint>
    {
        public readonly double Value;

        public abstract bool IncludesPoint { get; }

        public IntervalEndPoint(double value)
        {
            Value = value;
        }

        public static bool operator ==(IntervalEndPoint point1, IntervalEndPoint point2)
        {
            if (ReferenceEquals(point1, null))
                return ReferenceEquals(point2, null);

            return point1.Equals(point2);
        }

        public static bool operator !=(IntervalEndPoint point1, IntervalEndPoint point2)
            => !(point1 == point2);

        public bool Equals(IntervalEndPoint other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return Value == other.Value && IncludesPoint == other.IncludesPoint;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IntervalEndPoint);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Value.GetHashCode() + IncludesPoint.GetHashCode();
            }
        }
    }

    public class OpenIntervalEndPoint : IntervalEndPoint, IEquatable<OpenIntervalEndPoint>
    {
        public override bool IncludesPoint => false;

        public OpenIntervalEndPoint(double value)
            : base(value)
        { }

        public static bool operator==(OpenIntervalEndPoint point1, OpenIntervalEndPoint point2)
        {
            if (ReferenceEquals(point1, null))
                return ReferenceEquals(point2, null);

            return point1.Equals(point2);
        }

        public static bool operator !=(OpenIntervalEndPoint point1, OpenIntervalEndPoint point2)
            => !(point1 == point2);

        public bool Equals([AllowNull] OpenIntervalEndPoint other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return Value == other.Value;
        }
    }

    public class ClosedIntervalEndPoint : IntervalEndPoint, IEquatable<ClosedIntervalEndPoint>
    {
        public override bool IncludesPoint => true;

        public ClosedIntervalEndPoint(double value)
            : base(value)
        { }

        public static bool operator ==(ClosedIntervalEndPoint point1, ClosedIntervalEndPoint point2)
        {
            if (ReferenceEquals(point1, null))
                return ReferenceEquals(point2, null);

            return point1.Equals(point2);
        }

        public static bool operator !=(ClosedIntervalEndPoint point1, ClosedIntervalEndPoint point2)
            => !(point1 == point2);

        public bool Equals([AllowNull] ClosedIntervalEndPoint other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return Value == other.Value;
        }
    }


    [Serializable]
    public class InvalidIntervalException : Exception
    {
        public InvalidIntervalException() { }
        public InvalidIntervalException(IntervalEndPoint start, IntervalEndPoint end) : base(GetMessage(start, end)) { }
        public InvalidIntervalException(IntervalEndPoint start, IntervalEndPoint end, Exception inner) : base(GetMessage(start, end), inner) { }
        protected InvalidIntervalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        private static string GetMessage(IntervalEndPoint start, IntervalEndPoint end)
        {
            return $"The interval [{start.Value}, {end.Value}] is invalid because the end points were given in the wrong order";
        }
    }
}
