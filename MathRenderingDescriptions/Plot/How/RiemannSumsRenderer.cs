using AdobeComponents.Animation;
using AdobeComponents.Components;
using MathRenderingDescriptions.Plot.What;
using RenderingDescriptions.How;
using RenderingDescriptions.When;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MathRenderingDescriptions.Plot.How
{
    public class RiemannSumsRenderer : IHowToRender
    {
        private readonly RiemannSumsRenderingDescription _description;
        
        public RiemannSumsRenderer(RiemannSumsRenderingDescription description)
        {
            _description = description;
        }

        public RenderedComponents Render(AbsoluteTiming whenToRender)
        {
            var numTimeUnits = _description.NumTransitions + (_description.NumTransitions - 1) * _description.SplitMult;
            var timeUnit = _description.TotalDuration / numTimeUnits;

            var components = new List<TimedAdobeLayerComponent>();
            double currentTime = whenToRender.Time;

            for (int i = 0; i < _description.NumTransitions; i++)
            {
                var numRects = (int)Math.Pow(2, i);
                var splitLinesDuration = timeUnit * _description.SplitMult;

                if (i > 0)
                {
                    var splitLines = CreateSplitLines(numRects);
                    components.AddRange(CreateSplitLinesAnimation(splitLines,
                        currentTime,
                        splitLinesDuration));

                    currentTime += splitLinesDuration;
                }

                var sums = CreateRiemannSum(numRects);
                var sumsEndTime = currentTime + timeUnit;
                var sumsExtraTime = (i == _description.NumTransitions - 1 ? 0 : splitLinesDuration);
                if (i == 0)
                    components.AddRange(CreateBottomUpAnimation(sums, currentTime, sumsEndTime + sumsExtraTime));
                else
                    components.AddRange(CreateSplitSumsAnimation(sums, currentTime, sumsEndTime + sumsExtraTime));
                
                currentTime += sumsEndTime;
            }

            return new RenderedComponents(components);
        }

        private IEnumerable<TimedAdobeLayerComponent> CreateSplitLinesAnimation(List<SplitLine> splitLines,
            double startTime,
            double duration)
        {
            foreach (var splitLine in splitLines)
            {
                var topOnlyPoints = new PointF[]
                {
                    new PointF(splitLine.XValue, splitLine.Top),
                    new PointF(splitLine.XValue, splitLine.Top)
                };

                yield return new TimedAdobeLayerComponent(
                    new AdobePathComponent(
                        new AnimatedValue<PointF[]>(
                            new ValueAtTime<PointF[]>(topOnlyPoints, new AnimationTime(startTime)),
                            new ValueAtTime<PointF[]>(splitLine.GetPoints(), new AnimationTime(startTime + duration / 2)))),
                    startTime,
                    startTime + duration);
            }
        }

        private IEnumerable<TimedAdobeLayerComponent> CreateBottomUpAnimation(List<RiemannSumRect> rects,
            double startTime,
            double duration)
        {
            foreach (var rect in rects)
            {
                var endPoints = rect.GetPoints();
                var startPoints = new PointF[]
                {
                    new PointF(rect.Left, rect.Bottom),
                    new PointF(rect.Right, rect.Bottom),
                    new PointF(rect.Right, rect.Bottom),
                    new PointF(rect.Left, rect.Bottom)
                };

                yield return new TimedAdobeLayerComponent(
                    new AdobePathComponent(
                        new AnimatedValue<PointF[]>(
                            new ValueAtTime<PointF[]>(startPoints, new AnimationTime(startTime)),
                            new ValueAtTime<PointF[]>(endPoints, new AnimationTime(startTime + duration / 2))))
                        { IsClosed = true },
                    startTime,
                    startTime + duration);
            }
        }

        private IEnumerable<TimedAdobeLayerComponent> CreateSplitSumsAnimation(List<RiemannSumRect> rects,
            double startTime,
            double duration)
        {
            for (int i = 0; i < rects.Count; i++)
            {
                if (i % 2 == 0)
                {
                    var currentRect = rects[i];
                    var nextRect = rects[i + 1];

                    yield return new TimedAdobeLayerComponent(
                        new AdobePathComponent(
                            new AnimatedValue<PointF[]>(
                                new ValueAtTime<PointF[]>(nextRect.GetPoints(), new AnimationTime(startTime)),
                                new ValueAtTime<PointF[]>(currentRect.GetPoints(), new AnimationTime(startTime + duration / 2))))
                            { IsClosed = true },
                        startTime,
                        startTime + duration);
                }
                else
                {
                    yield return new TimedAdobeLayerComponent(
                        new AdobePathComponent(
                            new StaticValue<PointF[]>(rects[i].GetPoints()))
                            { IsClosed = true },
                        startTime,
                        startTime + duration);
                }
            }
        }

        private List<RiemannSumRect> CreateRiemannSum(int numRects)
        {
            var rectWidth = (_description.FunctionDescription.EndX - _description.FunctionDescription.StartX) / (numRects * 1.0);

            var result = new List<RiemannSumRect>();
            for (int i = 0; i < numRects; i++)
            {
                var leftX = rectWidth * i;
                var rightX = leftX + rectWidth;

                var visualBottomY = _description.FunctionDescription.PlotLayoutDescription.GetAxesIntersectionPoint().Y;
                var visualTopY = _description.FunctionDescription.PlotLayoutDescription.CreateFunctionPoint(
                    _description.FunctionDescription, rightX).Y;

                var visualLeftX = (float)_description.FunctionDescription.PlotLayoutDescription.GetVisualXValue(leftX);
                var visualRightX = (float)_description.FunctionDescription.PlotLayoutDescription.GetVisualXValue(rightX);

                result.Add(new RiemannSumRect(visualLeftX,
                    visualRightX,
                    visualTopY,
                    visualBottomY));
            }

            return result;
        }

        private List<SplitLine> CreateSplitLines(int numRects)
        {
            var rectWidth = (_description.FunctionDescription.EndX - _description.FunctionDescription.StartX) / (numRects * 1.0);

            var result = new List<SplitLine>();
            for (int i = 0; i < numRects; i++)
            {
                var leftX = rectWidth * i;
                var rightX = leftX + rectWidth;
                var lineX = (leftX + rightX) / 2;

                var visualBottomY = _description.FunctionDescription.PlotLayoutDescription.GetAxesIntersectionPoint().Y;
                var visualTopY = _description.FunctionDescription.PlotLayoutDescription.CreateFunctionPoint(
                    _description.FunctionDescription, rightX).Y;

                var visualLineX = (float)_description.FunctionDescription.PlotLayoutDescription.GetVisualXValue(lineX);

                result.Add(new SplitLine(visualTopY,
                    visualBottomY,
                    visualLineX));
            }

            return result;
        }

        private class SplitLine
        {
            public readonly float Top;
            public readonly float Bottom;
            public readonly float XValue;

            public SplitLine(float top,
                float bottom,
                float xValue)
            {
                Top = top;
                Bottom = bottom;
                XValue = xValue;
            }

            public PointF[] GetPoints()
            {
                return new PointF[]
                {
                    new PointF(XValue, Top),
                    new PointF(XValue, Bottom)
                };
            }
        }

        private class RiemannSumRect
        {
            public readonly float Left;
            public readonly float Right;
            public readonly float Top;
            public readonly float Bottom;

            public RiemannSumRect(float left,
                float right,
                float top,
                float bottom)
            {
                Left = left;
                Right = right;
                Top = top;
                Bottom = bottom;
            }

            public PointF[] GetPoints()
            {
                return new PointF[]
                {
                    new PointF(Left, Bottom),
                    new PointF(Right, Bottom),
                    new PointF(Right, Top),
                    new PointF(Left, Top)
                };
            }
        }

        private class RiemannSumResult
        {
            public readonly List<RiemannSumRect> Rects;
            public readonly double TotalArea;

            public RiemannSumResult(List<RiemannSumRect> rects,
                double totalArea)
            {
                Rects = rects;
                TotalArea = totalArea;
            }
        }
    }
}
