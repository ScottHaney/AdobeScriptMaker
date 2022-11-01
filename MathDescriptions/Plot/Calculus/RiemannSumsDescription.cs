using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot.Calculus
{
    public class RiemannSumsDescription
    {
        public readonly RiemannSumDescription RiemannSumStart;
        public readonly int NumTransitions;

        public RiemannSumsDescription(RiemannSumDescription riemannSumStart, int numTransitions)
        {
            RiemannSumStart = riemannSumStart;
            NumTransitions = numTransitions;
        }
    }
}
