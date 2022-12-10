using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot
{
    public class FunctionRenderingDescription : IWhatToRender
    {
        public readonly PlotLayoutDescription PlotLayoutDescription;
        public readonly Func<double, double> Function;

        public FunctionRenderingDescription(PlotLayoutDescription plotLayoutDescription,
            Func<double, double> function)
        {
            PlotLayoutDescription = plotLayoutDescription;
            Function = function;
        }
    }
}
