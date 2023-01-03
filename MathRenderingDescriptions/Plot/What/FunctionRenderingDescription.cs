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

        public FunctionRenderingDescription(string uniqueName,
            PlotLayoutDescription plotLayoutDescription,
            Func<double, double> function,
            double? startX = null,
            double? endX = null)
            : base(uniqueName)
        {
            PlotLayoutDescription = plotLayoutDescription;
            Function = function;

            StartX = startX ?? plotLayoutDescription.AxesLayout.XAxis.MinValue;
            EndX = endX ?? plotLayoutDescription.AxesLayout.XAxis.MaxValue;
        }
    }

    public class PolarFunctionRenderingDescription : IWhatToRender
    {
        public readonly PlotLayoutDescription PlotLayoutDescription;
        public readonly Func<double, double> Function;

        public double StartAngle { get; }
        public double EndAngle { get; }

        public PolarFunctionRenderingDescription(string uniqueName,
            PlotLayoutDescription plotLayoutDescription,
            Func<double, double> function,
            double startAngle,
            double endAngle)
            : base(uniqueName)
        {
            PlotLayoutDescription = plotLayoutDescription;
            Function = function;

            if (endAngle >= startAngle)
                throw new ArgumentException(nameof(endAngle));

            StartAngle = startAngle;
            EndAngle = endAngle;
        }
    }
}
