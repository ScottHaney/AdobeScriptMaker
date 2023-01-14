using AdobeComponents.Animation;
using AdobeComponents.CommonValues;
using AdobeComponents.Components;
using MathRenderingDescriptions.Plot.What.ArcLength;
using MathRenderingDescriptions.Plot.When;
using RenderingDescriptions.How;
using RenderingDescriptions.Timing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MathRenderingDescriptions.Plot.How.ArcLength
{
    public class ArcLengthRenderer : IHowToRender
    {
        private readonly ArcLengthRenderingDescription _description;

        public ArcLengthRenderer(ArcLengthRenderingDescription description)
        {
            _description = description;
        }

        public RenderedComponents Render(ITimingForRender timing)
        {
            var components = new List<TimedAdobeLayerComponent>();
            double currentTime = timing.WhenToStart.Time;

            var riemannSumsTimings = _description.TimingDescription.GetTimings(timing.WhenToStart.Time, timing.RenderDuration.Time);

            var index = 0;
            List<ArcLengthLine> previous = null;
            foreach (var riemannSumTiming in riemannSumsTimings)
            {
                var sums = CreateLines(riemannSumTiming.NumRects);

                var riemannSumsComponents = new List<TimedAdobeLayerComponent>();
                if (index == 0)
                    riemannSumsComponents.AddRange(CreateEntranceAnimations(sums, riemannSumTiming, _description.GetLinesColorControlName()));
                else
                    riemannSumsComponents.AddRange(CreateSplitLinesAnimation(sums, previous, riemannSumTiming, _description.GetLinesColorControlName()));

                
                foreach (var component in riemannSumsComponents)
                {
                    var pathGroup = (AdobePathGroupComponent)component.Component;

                    components.Add(new TimedAdobeLayerComponent(
                        pathGroup,
                        component.StartTime,
                        component.EndTime));
                }

                index++;
                previous = sums;
            }

            var linesColorControl = new AdobeSharedColorControl(_description.GetLinesColorControlName());
            components.Add(new TimedAdobeLayerComponent(linesColorControl, timing.WhenToStart.Time, currentTime));

            var strokeWidthControl = new AdobeSliderControl() { Name = _description.GetLinesStrokeWidthControlName() };
            components.Add(new TimedAdobeLayerComponent(strokeWidthControl, timing.WhenToStart.Time, currentTime));

            return new RenderedComponents(components);
        }

        public List<ArcLengthLine> CreateLines(int numSegments)
        {
            var lineWidth = (_description.FunctionDescription.EndX - _description.FunctionDescription.StartX) / (numSegments * 1.0);

            var result = new List<ArcLengthLine>();
            for (int i = 0; i < numSegments; i++)
            {
                var leftX = lineWidth * i;
                var rightX = leftX + lineWidth;

                var centerX = (leftX + rightX) / 2;
                var centerY = _description.FunctionDescription.Function(centerX);
                var slope = _description.DerivativeDescription.Function(centerX);

                var tangentLeftY = centerY - slope * (centerX - leftX);
                var tangentRightY = centerY + slope * (rightX - centerX);

                result.Add(
                    new ArcLengthLine(
                        new PointF(
                            (float)_description.FunctionDescription.PlotLayoutDescription.GetVisualXValue(leftX),
                            (float)_description.FunctionDescription.PlotLayoutDescription.GetVisualYValue(tangentLeftY)),
                        new PointF(
                            (float)_description.FunctionDescription.PlotLayoutDescription.GetVisualXValue(rightX),
                            (float)_description.FunctionDescription.PlotLayoutDescription.GetVisualYValue(tangentRightY)),
                        new ArcLengthMetadata(Math.Sqrt(Math.Pow((leftX - rightX), 2) + Math.Pow((tangentLeftY - tangentRightY), 2)))));
            }

            return result;
        }

        private IEnumerable<TimedAdobeLayerComponent> CreateEntranceAnimations(List<ArcLengthLine> lines,
            RiemannSumTimingResult timingResult,
            string colorControlName)
        {
            foreach (var line in lines)
            {
                var centerPoint = line.GetCenterPoint();

                var startPoints = new PointF[]
                {
                    centerPoint,
                    centerPoint
                };

                var endPoints = new PointF[]
                {
                    line.LeftPoint,
                    line.RightPoint
                };

                yield return new TimedAdobeLayerComponent(
                    new AdobePathGroupComponent(new AdobePathComponent(
                        new AnimatedValue<PointF[]>(
                            new ValueAtTime<PointF[]>(startPoints, new AnimationTime(timingResult.EntranceTime)),
                            new ValueAtTime<PointF[]>(endPoints, new AnimationTime(timingResult.SumIsInPlaceTime))))
                    {
                        IsClosed = true,
                        ColorValue = new AdobeColorControlRef("thisComp", "Shared Controls Layer", colorControlName),
                        StrokeWidth = new AdobeSliderControlRef(0, "thisComp", "Shared Controls Layer", _description.GetLinesStrokeWidthControlName())
                    }),
                    timingResult.EntranceTime,
                    timingResult.EndTime);
            }
        }

        private IEnumerable<TimedAdobeLayerComponent> CreateSplitLinesAnimation(List<ArcLengthLine> nextLines,
            List<ArcLengthLine> previousLines,
            RiemannSumTimingResult timingResult,
            string colorControlName)
        {
            var startingCopies = new List<ArcLengthLine>();
            foreach (var line in previousLines)
            {
                startingCopies.Add(new ArcLengthLine(line.LeftPoint,
                    line.GetCenterPoint(),
                    new ArcLengthMetadata(line.Metadata.Length / 2)));

                startingCopies.Add(new ArcLengthLine(line.GetCenterPoint(),
                    line.RightPoint,
                    new ArcLengthMetadata(line.Metadata.Length / 2)));
            }

            for (int i = 0; i < nextLines.Count; i++)
            {
                var startingCopy = startingCopies[i];
                var newLine = nextLines[i];

                yield return new TimedAdobeLayerComponent(
                        new AdobePathGroupComponent(new AdobePathComponent(
                            new AnimatedValue<PointF[]>(
                                new ValueAtTime<PointF[]>(startingCopy.GetPoints(), new AnimationTime(timingResult.EntranceTime)),
                                new ValueAtTime<PointF[]>(newLine.GetPoints(), new AnimationTime(timingResult.SumIsInPlaceTime))))
                        {
                            IsClosed = true,
                            ColorValue = new AdobeColorControlRef("thisComp", "Shared Controls Layer", colorControlName),
                            StrokeWidth = new AdobeSliderControlRef(0, "thisComp", "Shared Controls Layer", _description.GetLinesStrokeWidthControlName())
                        }),
                        timingResult.EntranceTime,
                        timingResult.EndTime);
            }
        }
    }

    public class ArcLengthLine
    {
        public readonly PointF LeftPoint;
        public readonly PointF RightPoint;
        public readonly ArcLengthMetadata Metadata;

        public ArcLengthLine(PointF leftPoint,
            PointF rightPoint,
            ArcLengthMetadata metadata)
        {
            LeftPoint = leftPoint;
            RightPoint = rightPoint;
            Metadata = metadata;
        }

        public PointF[] GetPoints()
        {
            return new PointF[] { LeftPoint, RightPoint };
        }

        public PointF GetCenterPoint()
        {
            return new PointF((LeftPoint.X + RightPoint.X) / 2, (LeftPoint.Y + RightPoint.Y) / 2);
        }
    }

    public class ArcLengthMetadata
    {
        public readonly double Length;

        public ArcLengthMetadata(double length)
        {
            Length = length;
        }
    }
}
