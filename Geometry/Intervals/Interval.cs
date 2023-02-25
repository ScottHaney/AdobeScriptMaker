using Geometry.Lines;
using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Intervals
{
    public class Interval
    {
        public readonly double Start;
        public readonly double End;

        public Interval(double start, double end)
        {
            Start = start;
            End = end;
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
