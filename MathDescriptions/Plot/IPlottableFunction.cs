using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot
{
    public interface IPlottableFunction
    {
        double GetYValue(double x);
    }
}
