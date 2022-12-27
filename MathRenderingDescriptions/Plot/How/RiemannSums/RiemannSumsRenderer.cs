using AdobeComponents.Animation;
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
        private readonly IRiemannSumsTiming _timingManager;
        
        public RiemannSumsRenderer(RiemannSumsRenderingDescription description,
            IRiemannSumsTiming timingManager)
        {
            _description = description;
            _timingManager = timingManager;
        }

        public RenderedComponents Render(ITimingForRender timing)
        {
            var components = new List<TimedAdobeLayerComponent>();
            double currentTime = timing.WhenToStart.Time;

            var scribbleColorControlName = "scribbleColorControl";
            var linesControlName = "linesColorControl";

            RiemannSumTiming previousTimingResult = null;
            foreach (var time in _timingManager.GetTimes(timing.WhenToStart.Time))
            {
                var sums = CreateRiemannSum(time.NumRects);
                var sumsEndTime = time.CompletionTime + time.PostCompletionSplitAnimationTime;

                var riemannSumsComponents = new List<TimedAdobeLayerComponent>();
                if (previousTimingResult == null)
                    riemannSumsComponents.AddRange(CreateBottomUpAnimation(sums, currentTime, sumsEndTime, linesControlName));
                else
                    riemannSumsComponents.AddRange(CreateSplitSumsAnimation(sums, currentTime, sumsEndTime, linesControlName));

                if (time.PostCompletionSplitAnimationTime > 0)
                {
                    var splitLines = CreateSplitLines(time.NumRects);
                    components.AddRange(CreateSplitLinesAnimation(splitLines,
                        time.CompletionTime,
                        time.PostCompletionSplitAnimationTime,
                        linesControlName));
                }

                foreach (var component in riemannSumsComponents)
                {
                    var maskName = "ScribbleMask";

                    var pathGroup = (AdobePathGroupComponent)component.Component;
                    foreach (var pathComponent in pathGroup.Paths)
                        pathComponent.Mask = new AdobeMaskComponent(pathComponent) { MaskName = maskName };

                    var scribble = new AdobeScribbleEffect(maskName)
                    {
                        ColorValue = new AdobeColorControlRef("thisComp", "Shared Controls Layer", scribbleColorControlName)
                    };
                    foreach (var pathComponent in pathGroup.Paths)
                        pathComponent.ScribbleEffect = scribble;

                    components.Add(new TimedAdobeLayerComponent(
                        pathGroup,
                        component.StartTime,
                        component.EndTime));
                }

                previousTimingResult = time;
            }

            var scribbleColorControl = new AdobeSharedColorControl(scribbleColorControlName);
            components.Add(new TimedAdobeLayerComponent(scribbleColorControl, timing.WhenToStart.Time, currentTime));

            var linesColorControl = new AdobeSharedColorControl(linesControlName);
            components.Add(new TimedAdobeLayerComponent(linesColorControl, timing.WhenToStart.Time, currentTime));

            return new RenderedComponents(components);
        }

        private IEnumerable<TimedAdobeLayerComponent> CreateSplitLinesAnimation(List<SplitLine> splitLines,
            double startTime,
            double duration,
            string colorControlName)
        {
            foreach (var splitLine in splitLines)
            {
                var topOnlyPoints = new PointF[]
                {
                    new PointF(splitLine.XValue, splitLine.Top),
                    new PointF(splitLine.XValue, splitLine.Top)
                };

                yield return new TimedAdobeLayerComponent(
                    new AdobePathGroupComponent(new AdobePathComponent(
                        new AnimatedValue<PointF[]>(
                            new ValueAtTime<PointF[]>(topOnlyPoints, new AnimationTime(startTime)),
                            new ValueAtTime<PointF[]>(splitLine.GetPoints(), new AnimationTime(startTime + duration / 2))))
                    {
                        ColorValue = new AdobeColorControlRef("thisComp", "Shared Controls Layer", colorControlName)
                    }),
                    startTime,
                    startTime + duration);
            }
        }

        private IEnumerable<TimedAdobeLayerComponent> CreateBottomUpAnimation(List<RiemannSumRect> rects,
            double startTime,
            double endTime,
            string colorControlName)
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
                    new AdobePathGroupComponent(new AdobePathComponent(
                        new AnimatedValue<PointF[]>(
                            new ValueAtTime<PointF[]>(startPoints, new AnimationTime(startTime)),
                            new ValueAtTime<PointF[]>(endPoints, new AnimationTime((startTime + endTime) / 2))))
                        {
                            IsClosed = true,
                            ColorValue = new AdobeColorControlRef("thisComp", "Shared Controls Layer", colorControlName)
                    }),
                    startTime,
                    endTime);
            }
        }

        private IEnumerable<TimedAdobeLayerComponent> CreateSplitSumsAnimation(List<RiemannSumRect> rects,
            double startTime,
            double endTime,
            string colorControlName)
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
                        new AdobePathGroupComponent(new AdobePathComponent(
                            new AnimatedValue<PointF[]>(
                                new ValueAtTime<PointF[]>(targetRect.GetPoints(), new AnimationTime(startTime)),
                                new ValueAtTime<PointF[]>(currentRect.GetPoints(), new AnimationTime((startTime + endTime) / 2))))
                            {
                                IsClosed = true,
                                ColorValue = new AdobeColorControlRef("thisComp", "Shared Controls Layer", colorControlName)
                        }),
                        startTime,
                        endTime);
                }
                else
                {
                    yield return new TimedAdobeLayerComponent(
                        new AdobePathGroupComponent(new AdobePathComponent(
                            new StaticValue<PointF[]>(rects[i].GetPoints()))
                            {
                                IsClosed = true,
                                ColorValue = new AdobeColorControlRef("thisComp", "Shared Controls Layer", colorControlName)
                            }),
                        startTime,
                        endTime);
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

    public interface IRiemannSumsTiming
    {
        IEnumerable<RiemannSumTiming> GetTimes(double startTime);
    }

    public class RiemannSumTiming
    {
        public readonly double CompletionTime;
        public readonly double PostCompletionSplitAnimationTime;
        public readonly int NumRects;

        public RiemannSumTiming(double completionTime,
            double postCompletionSplitAnimationTime,
            int numRects)
        {
            CompletionTime = completionTime;
            PostCompletionSplitAnimationTime = postCompletionSplitAnimationTime;
            NumRects = numRects;
        }
    }

    public class RiemannSumsTiming : IRiemannSumsTiming
    {
        private readonly RiemannSumsRenderingDescription _description;

        public RiemannSumsTiming(RiemannSumsRenderingDescription description)
        {
            _description = description;
        }

        public IEnumerable<RiemannSumTiming> GetTimes(double startTime)
        {
            var currentTime = startTime;
            foreach (var (index, numRects) in _description.SumsProvider.GetSums().Index())
            {
                var sumsEndTime = currentTime + _description.TimingDescription.GetTotalTimeForSum(index, numRects);
                var transitionAnimationDuration = _description.TimingDescription.GetTransitionAnimationTimeForSum(index, numRects);

                yield return new RiemannSumTiming(sumsEndTime + transitionAnimationDuration, transitionAnimationDuration, numRects);

                currentTime = sumsEndTime;
            }
        }
    }
}
