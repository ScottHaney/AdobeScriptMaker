using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.When
{
    public interface ITimingDescription
    {
        IEnumerable<RiemannSumTimingResult> GetTimings(double startTime, double duration);
    }

    public class RiemannSumTimingResult
    {
        public readonly double EntranceTime;
        public readonly double SumIsInPlaceTime;
        public readonly double EndTime;
        public readonly double? TransitionAnimationStartTime;
        public readonly int NumRects;

        public RiemannSumTimingResult(double entranceTime,
            double sumIsInPlaceTime,
            double endTime,
            double? transitionAnimationStartTime,
            int numRects)
        {
            EntranceTime = entranceTime;
            SumIsInPlaceTime = sumIsInPlaceTime;
            EndTime = endTime;
            TransitionAnimationStartTime = transitionAnimationStartTime;
            NumRects = numRects;
        }

    }
}
