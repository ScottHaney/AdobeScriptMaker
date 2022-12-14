using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What.RiemannSums
{
    public class SumsProvider : ISumsProvider
    {
        private readonly int[] _sums;
        public int NumSums => _sums.Length;

        public SumsProvider(params int[] sums)
        {
            _sums = sums ?? Array.Empty<int>();
        }

        public IEnumerable<int> GetSums()
        {
            return _sums;
        }
    }
}
