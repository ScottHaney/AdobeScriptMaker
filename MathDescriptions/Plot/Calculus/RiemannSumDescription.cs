using System;
using System.Collections.Generic;
using System.Text;

namespace MathDescriptions.Plot.Calculus
{
    public class RiemannSumDescription : IPlotDecoration
    {
        public readonly IPlottableFunction FunctionDescription;
        public readonly int NumRects;
        public readonly double StartX;
        public readonly double EndX;
        public readonly RiemannSumAnimationInfo AnimationInfo;

        public RiemannSumDescription(IPlottableFunction functionDescription,
            int numRects,
            double startX,
            double endX,
            RiemannSumAnimationInfo animationInfo = null)
        {
            FunctionDescription = functionDescription;
            NumRects = numRects;
            StartX = startX;
            EndX = endX;
            AnimationInfo = animationInfo;
        }
    }

    public class RiemannSumAnimationInfo
    {
        public readonly double AnimateStart;
        public readonly double AnimateEnd;

        public RiemannSumAnimationInfo(double animateStart,
            double animateEnd)
        {
            AnimateStart = animateStart;
            AnimateEnd = animateEnd;
        }
    }
}
