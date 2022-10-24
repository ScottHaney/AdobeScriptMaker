using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot.Functions
{
    public class AreaUnderFunctionDescription : IPlotDecoration
    {
        public readonly IPlottableFunction FunctionDescription;
        public readonly double StartX;
        public readonly double EndX;

        public AreaUnderFunctionDescription(IPlottableFunction functionDescription,
            double startX,
            double endX)
        {
            FunctionDescription = functionDescription;
            StartX = startX;
            EndX = endX;
        }
    }
}
