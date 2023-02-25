using Geometry.Lines;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Geometry.Intervals
{
    public class Interval
    {
        public readonly double Start;
        public readonly double End;

        public Interval(double start, double end)
        {
            if (start > end)
                throw new InvalidIntervalException(start, end);

            Start = start;
            End = end;
        }
    }


    [Serializable]
    public class InvalidIntervalException : Exception
    {
        public InvalidIntervalException() { }
        public InvalidIntervalException(double start, double end) : base(GetMessage(start, end)) { }
        public InvalidIntervalException(double start, double end, Exception inner) : base(GetMessage(start, end), inner) { }
        protected InvalidIntervalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        private static string GetMessage(double start, double end)
        {
            return $"The interval [{start},{end}] is invalid because the end points were given in the wrong order";
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
