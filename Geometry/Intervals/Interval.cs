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
    public class Interval : IEquatable<Interval>
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
            Interval left;
            Interval right;
            if (_start.Value < otherInterval._start.Value)
            {
                left = this;
                right = otherInterval;
            }
            else
            {
                left = otherInterval;
                right = this;
            }

            //Cases where the intervals have no overlap
            if (left._end.Value < right._start.Value)
                return Empty;
            if (left._start.Value > right._end.Value)
                return Empty;

            //Case where the intervals overlap at a single point
            if (left._end.Value == right._start.Value)
                return new Interval(left._end, right._start);

            //Cases where the intervals overlap to form a new interval
            var newStartPoint = GetPointByValue(Math.Max(left._start.Value, right._start.Value), left._start, right._start);
            var newEndPoint = GetPointByValue(Math.Min(left._end.Value, right._end.Value), left._end, right._end);

            return new Interval(newStartPoint, newEndPoint);
        }

        private IntervalEndPoint GetPointByValue(double targetValue, IntervalEndPoint option1, IntervalEndPoint option2)
        {
            if (option1.Value == targetValue && option2.Value == targetValue)
                return OverlapIdenticallyValuedPoints(option1, option2);

            return option1.Value == targetValue ? option1 : option2;
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

        public static bool operator==(Interval interval1, Interval interval2)
        {
            if (ReferenceEquals(interval1, null))
                return ReferenceEquals(interval2, null);

            return interval1.Equals(interval2);
        }

        public static bool operator !=(Interval interval1, Interval interval2)
            => !(interval1 == interval2);

        public bool Equals(Interval other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return _start == other._start && _end == other._end;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Interval);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _start.GetHashCode() + _end.GetHashCode();
            }
        }

        public override string ToString()
        {
            var startSymbol = _start.IncludesPoint ? "[" : "(";
            var endSymbol = _end.IncludesPoint ? "]" : ")";

            return $"{startSymbol}{_start.Value}, {_end.Value}{endSymbol}";
        }
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
