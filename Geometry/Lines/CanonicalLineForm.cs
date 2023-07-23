using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Geometry.Lines
{
    public class CanonicalLineForm : IEquatable<CanonicalLineForm>
    {
        public readonly double Slope;
        public readonly double YIntercept;

        public CanonicalLineForm(double slope, double yIntercept)
        {
            Slope = slope;
            YIntercept = yIntercept;
        }

        public static bool operator==(CanonicalLineForm x, CanonicalLineForm y)
        {
            if (ReferenceEquals(x, null))
                return ReferenceEquals(y, null);

            return x.Slope == y.Slope && x.YIntercept == y.YIntercept;
        }

        public static bool operator !=(CanonicalLineForm x, CanonicalLineForm y)
            => !(x == y);

        public bool Equals(CanonicalLineForm other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return Slope == other.Slope && YIntercept == other.YIntercept;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CanonicalLineForm);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Slope.GetHashCode() + YIntercept.GetHashCode();
            }
        }
    }
}
