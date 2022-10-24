using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot
{
    public class PlotDescription
    {
        public readonly AxisRangeDescription XAxis;
        public readonly AxisRangeDescription YAxis;
        public readonly IPlottable[] Functions;

        public PlotDescription(AxisRangeDescription xAxis,
            AxisRangeDescription yAxis,
            params IPlottable[] functions)
        {
            XAxis = xAxis;
            YAxis = yAxis;
            Functions = functions ?? Array.Empty<IPlottable>();
        }
    }
}
