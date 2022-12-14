using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What.RiemannSums
{
    public class FitToDuration : ITimingDescription
    {
        private readonly double _timePerSum;
        private readonly double _numSums;

        public double TransitionPercentage = 0.18;

        public FitToDuration(int numSums,
            double totalDuration)
        {
            _timePerSum = totalDuration / numSums;
            _numSums = numSums;
        }

        public double GetTotalTimeForSum(int sumIndex, int numRects)
        {
            return _timePerSum;
        }

        public double GetTransitionAnimationTimeForSum(int sumIndex, int numRects)
        {
            if (_numSums == sumIndex - 1)
                return 0;
            else
            {
                return GetTotalTimeForSum(sumIndex, numRects) * TransitionPercentage;
            }
        }
    }
}
