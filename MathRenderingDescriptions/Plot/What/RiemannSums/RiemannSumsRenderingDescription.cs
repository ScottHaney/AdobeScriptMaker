using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What.RiemannSums
{
    public class RiemannSumsRenderingDescription : IWhatToRender
    {
        public readonly FunctionRenderingDescription FunctionDescription;
        public readonly ITimingDescription TimingDescription;
        public readonly ISumsProvider SumsProvider;

        public RiemannSumsRenderingDescription(FunctionRenderingDescription functionDescription,
            ITimingDescription timingDescription,
            ISumsProvider sumsProvider)
        {
            FunctionDescription = functionDescription;
            TimingDescription = timingDescription;
            SumsProvider = sumsProvider;
        }
    }
}
