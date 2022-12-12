using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What
{
    public class FunctionRenderingDescription : IWhatToRender
    {
        public readonly PlotLayoutDescription PlotLayoutDescription;
        public readonly Func<double, double> Function;

        public double StartX { get; }
        public double EndX { get; }

        public FunctionRenderingDescription(PlotLayoutDescription plotLayoutDescription,
            Func<double, double> function,
            double? startX = null,
            double? endX = null)
        {
            PlotLayoutDescription = plotLayoutDescription;
            Function = function;

            StartX = startX ?? plotLayoutDescription.AxesLayout.XAxis.MinValue;
            EndX = endX ?? plotLayoutDescription.AxesLayout.XAxis.MaxValue;
        }
    }
}
