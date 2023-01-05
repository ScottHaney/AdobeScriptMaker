using MathRenderingDescriptions.Plot.What.RiemannSums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.When
{
    public class FitToDuration : ITimingDescription
    {
        private readonly ISumsProvider _sumsProvider;

        public double TransitionPercentage = 0.18;

        public FitToDuration(ISumsProvider sumsProvider)
        {
            _sumsProvider = sumsProvider;
        }

        public IEnumerable<RiemannSumTimingResult> GetTimings(double startTime,
            double duration)
        {
            var durationPerSum = duration / _sumsProvider.NumSums;

            var currentTime = startTime;
            var index = 0;
            foreach (var numRects in _sumsProvider.GetSums())
            {
                var endTime = currentTime + durationPerSum;
                yield return new RiemannSumTimingResult(currentTime,
                    (currentTime + endTime) / 2,
                    endTime,
                    index == _sumsProvider.NumSums - 1 ? (double?)null : (currentTime + durationPerSum * (1 - TransitionPercentage)),
                    numRects);

                currentTime = endTime;
                index++;
            }
        }
    }
}
