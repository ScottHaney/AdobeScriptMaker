using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot.Calculus
{
    public class RiemannSumDescription : IPlotDecoration
    {
        public readonly IPlottableFunction FunctionDescription;
        public readonly int NumRects;
        public readonly double StartX;
        public readonly double EndX;

        public RiemannSumDescription(IPlottableFunction functionDescription,
            int numRects,
            double startX,
            double endX)
        {
            FunctionDescription = functionDescription;
            NumRects = numRects;
            StartX = startX;
            EndX = endX;
        }
    }
}
