using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What.Helpers
{
    public interface IIntervalSegmentation
    {
        int NumSums { get; }
        IEnumerable<int> GetSums();
    }
}
