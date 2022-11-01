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
}
