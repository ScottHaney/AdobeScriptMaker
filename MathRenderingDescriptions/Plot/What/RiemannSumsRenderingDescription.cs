using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What
{
    public class RiemannSumsRenderingDescription : IWhatToRender
    {
        public readonly FunctionRenderingDescription FunctionDescription;
        public readonly int NumTransitions;
        public readonly double TotalDuration;
        public double SplitMult { get; set; } = 0.5;

        public RiemannSumsRenderingDescription(FunctionRenderingDescription functionDescription,
            int numTransitions,
            double totalDuration)
        {
            FunctionDescription = functionDescription;
            NumTransitions = numTransitions;
            TotalDuration = totalDuration;
        }
    }
}
