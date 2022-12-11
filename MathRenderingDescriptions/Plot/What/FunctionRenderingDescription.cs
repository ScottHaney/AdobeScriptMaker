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

        public double? StartX { get; set; }
        public double? EndX { get; set; }

        public FunctionRenderingDescription(PlotLayoutDescription plotLayoutDescription,
            Func<double, double> function)
        {
            PlotLayoutDescription = plotLayoutDescription;
            Function = function;
        }
    }
}
