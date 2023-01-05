using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What.Helpers
{
    public class IntervalSegmentation : IIntervalSegmentation
    {
        private readonly int[] _sums;
        public int NumSums => _sums.Length;

        public IntervalSegmentation(params int[] sums)
        {
            _sums = sums ?? Array.Empty<int>();
        }

        public IEnumerable<int> GetSums()
        {
            return _sums;
        }
    }
}
