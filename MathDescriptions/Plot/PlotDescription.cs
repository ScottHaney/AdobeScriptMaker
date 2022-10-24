using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot
{
    public class PlotDescription
    {
        public readonly AxisRangeDescription XAxis;
        public readonly AxisRangeDescription YAxis;
        public readonly IPlottableFunction[] Functions;

        public readonly List<IPlotDecoration> Decorations = new List<IPlotDecoration>();

        public PlotDescription(AxisRangeDescription xAxis,
            AxisRangeDescription yAxis,
            params IPlottableFunction[] functions)
        {
            XAxis = xAxis;
            YAxis = yAxis;
            Functions = functions ?? Array.Empty<IPlottableFunction>();
        }
    }
}
