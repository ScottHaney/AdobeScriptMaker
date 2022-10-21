using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot
{
    public class AxisRangeDescription
    {
        public readonly double MinValue;
        public readonly double MaxValue;

        public AxisRangeDescription(double minValue, double maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
