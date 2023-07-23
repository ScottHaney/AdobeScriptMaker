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
        public readonly IntervalEndPoint Start;
        public readonly IntervalEndPoint End;

        public Interval(IntervalEndPoint startPoint, IntervalEndPoint endPoint)
        {
            if (startPoint.Value > endPoint.Value)
                throw new InvalidIntervalException(startPoint, endPoint);

            Start = startPoint;
            End = endPoint;
        }

        public static Interval CreateClosedInterval(double start, double end)
            => new Interval(new ClosedIntervalEndPoint(start), new ClosedIntervalEndPoint(end));

        public static Interval CreateOpenInterval(double start, double end)
            => new Interval(new OpenIntervalEndPoint(start), new OpenIntervalEndPoint(end));

        public static readonly Interval Empty = Interval.CreateOpenInterval(0, 0);

        public IntervalExclusionResult Exclude(IEnumerable<Interval> otherIntervals)
        {
            IntervalExclusionResult result = this;
            foreach (var otherInterval in otherIntervals)
            {
                if (result.IsEmpty)
                    break;

                result = new IntervalExclusionResult(result.NonEmptyIntervals
                    .SelectMany(x => x.Exclude(otherInterval).NonEmptyIntervals)
                    .ToArray());
            }

            return result;
        }

        public IntervalExclusionResult Exclude(Interval otherInterval)
        {
            var intersection = IntersectionWith(otherInterval);
            if (!intersection.IsEmpty())
            {
                var results = new List<Interval>();
                if (Start.Value <= intersection.Start.Value)
                    results.Add(new Interval(Start, intersection.Start.GetOppositePoint()));
                if (intersection.End.Value <= End.Value)
                    results.Add(new Interval(intersection.End.GetOppositePoint(), End));

                return new IntervalExclusionResult(results.ToArray());
            }
            else
                return this;
        }

        public Interval IntersectionWith(Interval otherInterval)
        {
            Interval left;
            Interval right;
            if (Start.Value < otherInterval.Start.Value)
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
            if (left.End.Value < right.Start.Value)
                return Empty;
            if (left.Start.Value > right.End.Value)
                return Empty;

            //Case where the intervals overlap at a single point
            if (left.End.Value == right.Start.Value)
                return new Interval(left.End, right.Start);

            //Cases where the intervals overlap to form a new interval
            var newStartPoint = GetPointByValue(Math.Max(left.Start.Value, right.Start.Value), left.Start, right.Start);
            var newEndPoint = GetPointByValue(Math.Min(left.End.Value, right.End.Value), left.End, right.End);

            return new Interval(newStartPoint, newEndPoint);
        }

        public bool TryConnectWith(Interval otherInterval, out Interval combinedInterval)
        {
            var intersection = this.IntersectionWith(otherInterval);
            if (intersection.ContainsSinglePoint())
            {
                if (End.Value == otherInterval.Start.Value)
                {
                    combinedInterval = new Interval(Start, otherInterval.End);
                    return true;
                }
                else if (Start.Value == otherInterval.End.Value)
                {
                    combinedInterval = new Interval(otherInterval.Start, End);
                    return true;
                }
            }

            combinedInterval = null;
            return false;
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

        public bool ContainsMoreThanOnePoint()
        {
            return !IsEmpty() && !ContainsSinglePoint();
        }

        public bool IsEmpty()
        {
            if (End.Value > Start.Value)
                return false;
            else
                return !IncludesValue(End.Value);
        }

        public bool ContainsSinglePoint()
        {
            if (End.Value != Start.Value)
                return false;
            else
                return End.IncludesPoint && Start.IncludesPoint;
        }

        private bool IncludesValue(double value)
        {
            if (value > Start.Value && value < End.Value)
                return true;
            else if (value < Start.Value || value > End.Value)
                return false;
            else
            {
                //Make sure that each end point that matches the value (could be both end points in an interval like [9,9]) includes the point
                return new[] { Start, End }
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

            return Start == other.Start && End == other.End;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Interval);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Start.GetHashCode() + End.GetHashCode();
            }
        }

        public override string ToString()
        {
            var startSymbol = Start.IncludesPoint ? "[" : "(";
            var endSymbol = End.IncludesPoint ? "]" : ")";

            return $"{startSymbol}{Start.Value}, {End.Value}{endSymbol}";
        }

        public static implicit operator IntervalExclusionResult(Interval interval) => new IntervalExclusionResult(interval);
    }

    public class IntervalExclusionResult
    {
        private readonly Interval[] _resultingIntervals;

        public static readonly IntervalExclusionResult Empty = new IntervalExclusionResult();

        public bool IsEmpty => _resultingIntervals.Length == 0 || _resultingIntervals.All(x => x.IsEmpty());

        public bool HasPositiveLengthIntervals => _resultingIntervals.Any(x => x.ContainsMoreThanOnePoint());

        public IEnumerable<Interval> NonEmptyIntervals => _resultingIntervals.Where(x => !x.IsEmpty());

        public IEnumerable<Interval> PositiveLengthIntervals => _resultingIntervals.Where(x => x.ContainsMoreThanOnePoint());

        public IntervalExclusionResult(params Interval[] resultingIntervals)
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

        public abstract IntervalEndPoint GetOppositePoint();

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

        public override IntervalEndPoint GetOppositePoint()
        {
            return new ClosedIntervalEndPoint(Value);
        }

        public static bool operator==(OpenIntervalEndPoint point1, OpenIntervalEndPoint point2)
        {
            if (ReferenceEquals(point1, null))
                return ReferenceEquals(point2, null);

            return point1.Equals(point2);
        }

        public static bool operator !=(OpenIntervalEndPoint point1, OpenIntervalEndPoint point2)
            => !(point1 == point2);

        public bool Equals(OpenIntervalEndPoint other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ClosedIntervalEndPoint : IntervalEndPoint, IEquatable<ClosedIntervalEndPoint>
    {
        public override bool IncludesPoint => true;

        public ClosedIntervalEndPoint(double value)
            : base(value)
        { }

        public override IntervalEndPoint GetOppositePoint()
        {
            return new OpenIntervalEndPoint(Value);
        }

        public static bool operator ==(ClosedIntervalEndPoint point1, ClosedIntervalEndPoint point2)
        {
            if (ReferenceEquals(point1, null))
                return ReferenceEquals(point2, null);

            return point1.Equals(point2);
        }

        public static bool operator !=(ClosedIntervalEndPoint point1, ClosedIntervalEndPoint point2)
            => !(point1 == point2);

        public bool Equals(ClosedIntervalEndPoint other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
