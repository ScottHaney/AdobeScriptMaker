using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot.Functions
{
    public class PolynomialDescription : IPlottable
    {
        public readonly PolynomialTermDescription[] Terms;

        public PolynomialDescription(params PolynomialTermDescription[] terms)
        {
            Terms = terms ?? Array.Empty<PolynomialTermDescription>();
        }

        public double GetYValue(double x)
        {
            double total = 0;
            foreach (var term in Terms)
                total += term.Coefficient * Math.Pow(x, term.Power);

            return total;
        }
    }

    public class PolynomialTermDescription
    {
        public readonly double Coefficient;
        public readonly int Power;

        public PolynomialTermDescription(double coefficient, int power)
        {
            Coefficient = coefficient;
            Power = power;
        }
    }
}
