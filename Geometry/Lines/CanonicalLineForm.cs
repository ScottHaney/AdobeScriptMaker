using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Lines
{
    public class CanonicalLineForm
    {
        public readonly double Slope;
        public readonly double YIntercept;

        public CanonicalLineForm(double slope, double yIntercept)
        {
            Slope = slope;
            YIntercept = yIntercept;
        }
    }
}
