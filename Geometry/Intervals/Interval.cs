using Geometry.Lines;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
            if (startPoint > endPoint)
                throw new InvalidIntervalException(startPoint, endPoint);

            _start = startPoint;
            _end = endPoint;
        }

        public static Interval CreateClosedInterval(double start, double end)
            => new Interval(new ClosedIntervalEndPoint(start), new ClosedIntervalEndPoint(end));

        public static Interval CreateOpenInterval(double start, double end)
            => new Interval(new OpenIntervalEndPoint(start), new OpenIntervalEndPoint(end));
    }

    public abstract class IntervalEndPoint
    {
        public readonly double Value;

        public abstract bool IncludesPoint { get; }

        public IntervalEndPoint(double value)
        {
            Value = value;
        }

        public static bool operator>(IntervalEndPoint point, IntervalEndPoint otherPoint)
        {
            return point.Value > otherPoint.Value;
        }

        public static bool operator <(IntervalEndPoint point, IntervalEndPoint otherPoint)
        {
            return point.Value < otherPoint.Value;
        }
    }

    public class OpenIntervalEndPoint : IntervalEndPoint
    {
        public override bool IncludesPoint => false;

        public OpenIntervalEndPoint(double value)
            : base(value)
        { }
    }

    public class ClosedIntervalEndPoint : IntervalEndPoint
    {
        public override bool IncludesPoint => true;

        public ClosedIntervalEndPoint(double value)
            : base(value)
        { }
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

    /*public class ParametricRange
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
    }*/
}
