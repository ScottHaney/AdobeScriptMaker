﻿using AdobeComponents.Animation;
using AdobeComponents.CommonValues;
using AdobeComponents.Components;
using AdobeComponents.Effects;
using MathRenderingDescriptions.Plot.What;
using MathRenderingDescriptions.Plot.What.RiemannSums;
using RenderingDescriptions.How;
using RenderingDescriptions.When;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MoreLinq;
using RenderingDescriptions.Timing;

namespace MathRenderingDescriptions.Plot.How.RiemannSums
{
    public class RiemannSumsRenderer : IHowToRender
    {
        private readonly RiemannSumsRenderingDescription _description;
        
        public RiemannSumsRenderer(RiemannSumsRenderingDescription description)
        {
            _description = description;
        }

        public RenderedComponents Render(ITimingForRender timing)
        {
            var components = new List<TimedAdobeLayerComponent>();
            double currentTime = timing.WhenToStart.Time;

            var sharedColorControlName = "riemannSumsColorControl";
            var previousNumRects = 0;
            foreach (var (index, numRects) in _description.SumsProvider.GetSums().Index())
            {
                var sums = CreateRiemannSum(numRects);
                var sumsEndTime = currentTime + _description.TimingDescription.GetTotalTimeForSum(index, numRects);

                var riemannSumsComponents = new List<TimedAdobeLayerComponent>();
                if (index == 0)
                    riemannSumsComponents.AddRange(CreateBottomUpAnimation(sums, currentTime, sumsEndTime));
                else
                    riemannSumsComponents.AddRange(CreateSplitSumsAnimation(sums, currentTime, sumsEndTime));

                var transitionAnimationDuration = _description.TimingDescription.GetTransitionAnimationTimeForSum(index, numRects);
                if (transitionAnimationDuration > 0)
                {
                    var splitLines = CreateSplitLines(previousNumRects);
                    components.AddRange(CreateSplitLinesAnimation(splitLines,
                        currentTime - transitionAnimationDuration,
                        transitionAnimationDuration));
                }

                foreach (var component in riemannSumsComponents)
                {
                    var mask = new AdobeMaskComponent((AdobePathComponent)component.Component) { MaskName = "ScribbleMask" };
                    var scribble = new AdobeScribbleEffect(mask.MaskName)
                    {
                        ColorValue = new AdobeColorControlRef("thisComp", "Shared Controls Layer", sharedColorControlName)
                    };

                    components.Add(new TimedAdobeLayerComponent(
                        new GroupedTogetherAdobeLayerComponents(component.Component, mask, scribble),
                        component.StartTime,
                        component.EndTime));
                }
                
                currentTime += sumsEndTime;
                previousNumRects = numRects;
            }

            var colorControl = new AdobeSharedColorControl(sharedColorControlName);
            components.Add(new TimedAdobeLayerComponent(colorControl, timing.WhenToStart.Time, currentTime));

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

                    //Setting metadata is a little suspicious here and it will probably never
                    //be used so just set it to null and have it blow up if it ever is used...
                    var targetRect = new RiemannSumRect(currentRect.Left,
                        currentRect.Right,
                        nextRect.Top,
                        nextRect.Bottom,
                        null);

                    yield return new TimedAdobeLayerComponent(
                        new AdobePathComponent(
                            new AnimatedValue<PointF[]>(
                                new ValueAtTime<PointF[]>(targetRect.GetPoints(), new AnimationTime(startTime)),
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

                var xValueForTop = rightX;

                var visualBottomY = _description.FunctionDescription.PlotLayoutDescription.GetAxesIntersectionPoint().Y;
                var topY = _description.FunctionDescription.Function(xValueForTop);
                var visualTopY = _description.FunctionDescription.PlotLayoutDescription.CreateFunctionPoint(
                    _description.FunctionDescription, xValueForTop).Y;

                var visualLeftX = (float)_description.FunctionDescription.PlotLayoutDescription.GetVisualXValue(leftX);
                var visualRightX = (float)_description.FunctionDescription.PlotLayoutDescription.GetVisualXValue(rightX);

                result.Add(new RiemannSumRect(visualLeftX,
                    visualRightX,
                    visualTopY,
                    visualBottomY,
                    new RiemannSumRectMetadata(Math.Abs((rightX - leftX) * topY))));
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
            public readonly RiemannSumRectMetadata Metadata;

            public RiemannSumRect(float left,
                float right,
                float top,
                float bottom,
                RiemannSumRectMetadata metadata)
            {
                Left = left;
                Right = right;
                Top = top;
                Bottom = bottom;
                Metadata = metadata;
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

        private class RiemannSumRectMetadata
        {
            public readonly double Area;

            public RiemannSumRectMetadata(double area)
            {
                Area = area;
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
