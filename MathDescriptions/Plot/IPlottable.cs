using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot
{
    public interface IPlottable
    {
        double GetYValue(double x);
    }
}
