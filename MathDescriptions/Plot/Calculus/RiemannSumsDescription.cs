using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot.Calculus
{
    public class RiemannSumsDescription : IPlotDecoration
    {
        public readonly RiemannSumDescription RiemannSumStart;
        public readonly int NumSums;

        public RiemannSumsDescription(RiemannSumDescription riemannSumStart, int numSums)
        {
            RiemannSumStart = riemannSumStart;
            NumSums = numSums;
        }
    }

    public class RiemannSumsMetadata
    {
        public readonly RiemannSumStartTime[] StartTimes;

        public RiemannSumsMetadata(params RiemannSumStartTime[] startTimes)
        {
            StartTimes = startTimes ?? Array.Empty<RiemannSumStartTime>();
        }
    }

    public class RiemannSumStartTime
    {
        public readonly double Time;
        public readonly int NumRectangles;

        public RiemannSumStartTime(double time,
            int numRectangles)
        {
            Time = time;
            NumRectangles = numRectangles;
        }
    }
}
