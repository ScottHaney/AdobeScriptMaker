﻿using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;
using MoreLinq;
using MathRenderingDescriptions.Plot.When;
using MathRenderingDescriptions.Plot.What.Helpers;

namespace MathRenderingDescriptions.Plot.What.RiemannSums
{
    public class RiemannSumsRenderingDescription : IWhatToRender
    {
        public readonly FunctionRenderingDescription FunctionDescription;
        public readonly ITimingDescription TimingDescription;
        public readonly IIntervalSegmentation SumsProvider;

        public RiemannSumsRenderingDescription(string uniqueName,
            FunctionRenderingDescription functionDescription,
            ITimingDescription timingDescription,
            IIntervalSegmentation sumsProvider)
            : base(uniqueName)
        {
            FunctionDescription = functionDescription;
            TimingDescription = timingDescription;
            SumsProvider = sumsProvider;
        }

        public RiemannSumsMetadata GetMetadata()
        {
            var sumsResults = new List<RiemannSumMetadata>();

            foreach (var numRects in SumsProvider.GetSums())
            {
                sumsResults.Add(new RiemannSumMetadata(GetArea(numRects),
                    numRects));
            }

            return new RiemannSumsMetadata(sumsResults.ToArray());
        }

        public string GetScribbleColorControlName()
            => $"{UniqueName} - scribble color";

        public string GetLinesColorControlName()
            => $"{UniqueName} - lines color";

        public string GetWigglesPerSecondControlName()
            => $"{UniqueName} - wiggles";

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

        public RiemannSumMetadata(double totalArea,
            int numSums)
        {
            TotalArea = totalArea;
            NumSums = numSums;
        }
    }
}
