using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;
using MoreLinq;

namespace MathRenderingDescriptions.Plot.What.RiemannSums
{
    public class RiemannSumsRenderingDescription : IWhatToRender
    {
        public readonly FunctionRenderingDescription FunctionDescription;
        public readonly ITimingDescription TimingDescription;
        public readonly ISumsProvider SumsProvider;

        public RiemannSumsRenderingDescription(FunctionRenderingDescription functionDescription,
            ITimingDescription timingDescription,
            ISumsProvider sumsProvider)
        {
            FunctionDescription = functionDescription;
            TimingDescription = timingDescription;
            SumsProvider = sumsProvider;
        }

        public RiemannSumsMetadata GetMetadata()
        {
            var sumsResults = new List<RiemannSumMetadata>();
            double currentTime = 0;

            foreach (var (index, numRects) in SumsProvider.GetSums().Index())
            {
                var duration = TimingDescription.GetTotalTimeForSum(index, numRects);
                sumsResults.Add(new RiemannSumMetadata(GetArea(numRects),
                    numRects,
                    currentTime,
                    currentTime + duration));

                currentTime += duration;
            }

            return new RiemannSumsMetadata(sumsResults.ToArray());
        }

        private double GetArea(int numRects)
        {
            double area = 0;

            var rectWidth = (FunctionDescription.EndX - FunctionDescription.StartX) / numRects;
            for (int i = 0; i < numRects; i++)
            {
                var height = FunctionDescription.Function((i + 1) * rectWidth);
                area += height * rectWidth;
            }

            return area;
        }
    }

    public class RiemannSumsMetadata
    {
        public readonly RiemannSumMetadata[] SumsDetails;

        public RiemannSumsMetadata(params RiemannSumMetadata[] sumsDetails)
        {
            SumsDetails = sumsDetails ?? Array.Empty<RiemannSumMetadata>();
        }
    }

    public class RiemannSumMetadata
    {
        public readonly double TotalArea;
        public readonly int NumSums;
        public readonly double StartTimeOffset;
        public readonly double EndTimeOffset;

        public RiemannSumMetadata(double totalArea,
            int numSums,
            double startTimeOffset,
            double endTimeOffset)
        {
            TotalArea = totalArea;
            NumSums = numSums;
            StartTimeOffset = startTimeOffset;
            EndTimeOffset = endTimeOffset;
        }
    }
}
