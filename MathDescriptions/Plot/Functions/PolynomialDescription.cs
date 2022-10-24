using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot.Functions
{
    public class FunctionDescription : IPlottable
    {
        private readonly Func<double, double> _function;

        public FunctionDescription(Func<double, double> function)
        {
            _function = function;
        }

        public double GetYValue(double x)
        {
            return _function(x);
        }
    }
}
