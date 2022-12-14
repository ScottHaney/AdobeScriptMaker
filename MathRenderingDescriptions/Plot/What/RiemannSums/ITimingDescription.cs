using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What.RiemannSums
{
    public interface ITimingDescription
    {
        double GetTotalTimeForSum(int sumIndex, int numRects);
        double GetTransitionAnimationTimeForSum(int sumIndex, int numRects);
    }
}
