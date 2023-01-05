using MathRenderingDescriptions.Plot.When;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What.ArcLength
{
    public class ArcLengthRenderingDescription : IWhatToRender
    {
        public readonly FunctionRenderingDescription FunctionDescription;
        public readonly FunctionRenderingDescription DerivativeDescription;
        public readonly ITimingDescription TimingDescription;

        public ArcLengthRenderingDescription(string uniqueName,
            FunctionRenderingDescription functionDescription,
            FunctionRenderingDescription derivativeDescription,
            ITimingDescription timingDescription)
            : base(uniqueName)
        {
            FunctionDescription = functionDescription;
            DerivativeDescription = derivativeDescription;
            TimingDescription = timingDescription;
        }
    }
}
