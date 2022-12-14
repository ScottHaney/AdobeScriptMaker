using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What.RiemannSums
{
    public interface ISumsProvider
    {
        int NumSums { get; }
        IEnumerable<int> GetSums();
    }
}
