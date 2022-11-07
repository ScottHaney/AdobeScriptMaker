using MathDescriptions.Plot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DirectRendering.Drawing;
using MathDescriptions.Plot.Functions;
using System.Linq;
using MathDescriptions.Plot.Calculus;
using DirectRendering.Drawing.Animation;
using DirectRendering.Text;

namespace DirectRendering.Plotting
{
    public class Plot : IDrawing
    {
        private readonly PlotDescription _plotDescription;
        private readonly Rectangle _visualBounds;

        public Plot(PlotDescription plotDescription, Rectangle visualBounds)
        {
            _plotDescription = plotDescription;
            _visualBounds = visualBounds;
        }

        public IEnumerable<IDrawing> GetDrawings()
        {
            var axes = new PlotAxes(_visualBounds);

            foreach (var function in _plotDescription.Functions)
                yield return CreateFunctionDrawing(function, _visualBounds, _plotDescription);

            foreach (var decorator in _plotDescription.Decorations.OfType<AreaUnderFunctionDescription>())
                yield return CreateAreaUnderFunctionDrawing(decorator, _visualBounds, _plotDescription);

            foreach (var decorator in _plotDescription.Decorations.OfType<RiemannSumDescription>())
            {
                foreach (var path in CreateRiemannSum_BottomUp(decorator, _visualBounds, _plotDescription).Rects)
                    yield return path.Drawing;
            }

            foreach (var decorator in _plotDescription.Decorations.OfType<RiemannSumsDescription>())
            {
                var riemannSumsResult = CreateRiemannSums(decorator, _visualBounds, _plotDescription);
                foreach (var path in riemannSumsResult.Drawings)
                    yield return path;
            }

            foreach (var drawing in axes.GetDrawings())
                yield return drawing;
        }

        private RiemannSumsResult CreateRiemannSums(RiemannSumsDescription riemannSums,
            Rectangle axisRect,
            PlotDescription plotDescription)
        {
            var currentSumDescription = riemannSums.RiemannSumStart;
            var currentTime = 0;

            var sliderValues = new List<SliderValue>();
            var startTimes = new List<RiemannSumStartTime>();

            var drawings = new List<IDrawing>();
            var sequenceValues = new List<SequenceValue>();

            for (int i = 1; i <= riemannSums.NumSums; i++)
            {
                var animationStartTime = currentTime + 0.5;
                var sumsInPlaceTime = currentTime + 1.5;
                var splitLineTime = currentTime + 3;
                var endTime = currentTime + 4;

                RiemannSumResult currentRiemannSum;
                if (i == 1)
                    currentRiemannSum = CreateRiemannSum_BottomUp(currentSumDescription, axisRect, plotDescription);
                else
                    currentRiemannSum = CreateRiemannSum_SplitTopDown(currentSumDescription, axisRect, plotDescription, animationStartTime, sumsInPlaceTime);

                drawings.Add(new TimingContext(new AbsoluteTimingContextTime(currentTime),
                    new AbsoluteTimingContextTime(riemannSums.NumSums == i ? 30 : 4),
                    currentRiemannSum.Rects.Select(x => x.Drawing).ToArray()));

                if (riemannSums.NumSums != i)
                {
                    var lines = currentRiemannSum.Rects.Select(x => CreateSplitLine(x, splitLineTime, endTime)).ToArray();
                    drawings.Add(new TimingContext(new AbsoluteTimingContextTime(currentTime + 3),
                        new AbsoluteTimingContextTime(1),
                        lines));
                }

                sliderValues.Add(new SliderValue(currentTime, currentSumDescription.NumRects));
                startTimes.Add(new RiemannSumStartTime(currentTime, currentSumDescription.NumRects));

                var nextRiemannSumDescription = new RiemannSumDescription(riemannSums.RiemannSumStart.FunctionDescription,
                    currentSumDescription.NumRects * 2,
                    riemannSums.RiemannSumStart.StartX,
                    riemannSums.RiemannSumStart.EndX);

                if (i == 1)
                    sequenceValues.Add(new SequenceValue(currentRiemannSum.TotalArea, riemannSums.RiemannSumStart.AnimationInfo.AnimateEnd));
                else
                    sequenceValues.Add(new SequenceValue(currentRiemannSum.TotalArea, sumsInPlaceTime));

                currentSumDescription = nextRiemannSumDescription;
                currentTime = endTime;
            }

            drawings.Add(new SliderControl(sliderValues.ToArray()));
            drawings.Add(new SequenceDrawing("Areas:", riemannSums.RiemannSumStart.AnimationInfo.AnimateStart, sequenceValues.ToArray()));

            return new RiemannSumsResult(drawings,
                new RiemannSumsMetadata(startTimes.ToArray()));
        }

        private class RiemannSumsResult
        {
            public IEnumerable<IDrawing> Drawings;
            public RiemannSumsMetadata Metadata;

            public RiemannSumsResult(IEnumerable<IDrawing> drawings,
                RiemannSumsMetadata metadata)
            {
                Drawings = drawings;
                Metadata = metadata;
            }
        }

        private PathDrawing CreateSplitLine(RiemannSumRect RiemannSumRect,
            double animationStart,
            double animationEnd)
        {
            var midpointX = RiemannSumRect.BoundingRect.Left + RiemannSumRect.BoundingRect.Width / 2;

            var startPoints = new PointF[]
            {
                new PointF(midpointX, RiemannSumRect.BoundingRect.Top),
                new PointF(midpointX, RiemannSumRect.BoundingRect.Top)
            };

            var endPoints = new PointF[]
            {
                new PointF(midpointX, RiemannSumRect.BoundingRect.Top),
                new PointF(midpointX, RiemannSumRect.BoundingRect.Bottom)
            };

            return new PathDrawing(new AnimatedValue<PointF[]>(
                        new ValueAtTime<PointF[]>(startPoints, new AnimationTime(animationStart)),
                        new ValueAtTime<PointF[]>(endPoints, new AnimationTime(animationEnd))));
        }

        private RiemannSumResult CreateRiemannSum_BottomUp(RiemannSumDescription riemannSum,
            Rectangle axisRect,
            PlotDescription plotDescription)
        {
            var totalArea = 0d;
            var rects = new List<RiemannSumRect>();

            var rectWidth = (riemannSum.EndX - riemannSum.StartX) / riemannSum.NumRects;
            for (int i = 0; i < riemannSum.NumRects; i++)
            {
                var rightX = rectWidth * (i + 1);
                var leftX = rightX - rectWidth;
                var topY = riemannSum.FunctionDescription.GetYValue(rightX);
                var bottomY = 0d;

                if (topY < bottomY)
                {
                    var temp = topY;
                    topY = bottomY;
                    bottomY = temp;
                }

                totalArea += topY * (rightX - leftX);

                var visualRightX = (int)GetVisualXValue(rightX, plotDescription.XAxis, axisRect);
                var visualLeftX = (int)GetVisualXValue(leftX, plotDescription.XAxis, axisRect);
                var visualTopY = (int)GetVisualYValue(topY, plotDescription.YAxis, axisRect);
                var visualBottomY = (int)GetVisualYValue(bottomY, plotDescription.YAxis, axisRect);

                PathDrawing drawing;

                var points = new PointF[]
                    {
                        new PointF(visualLeftX, visualTopY),
                        new PointF(visualRightX, visualTopY),
                        new PointF(visualRightX, visualBottomY),
                        new PointF(visualLeftX, visualBottomY)
                    };

                if (riemannSum.AnimationInfo == null)
                {
                    drawing = new PathDrawing(points);
                }
                else
                {
                    var startPoints = new PointF[]
                    {
                        new PointF(visualLeftX, visualBottomY),
                        new PointF(visualRightX, visualBottomY),
                        new PointF(visualRightX, visualBottomY),
                        new PointF(visualLeftX, visualBottomY)
                    };

                    drawing = new PathDrawing(new AnimatedValue<PointF[]>(
                        new ValueAtTime<PointF[]>(startPoints, new AnimationTime(riemannSum.AnimationInfo.AnimateStart)),
                        new ValueAtTime<PointF[]>(points, new AnimationTime(riemannSum.AnimationInfo.AnimateEnd))));
                }
                
                drawing.IsClosed = true;
                drawing.HasLockedScale = false;

                rects.Add(new RiemannSumRect(drawing,
                    new RectangleF(visualLeftX, visualTopY, (visualRightX - visualLeftX), (visualBottomY - visualTopY))));
            }

            return new RiemannSumResult(rects, totalArea);
        }

        private RiemannSumResult CreateRiemannSum_SplitTopDown(RiemannSumDescription riemannSum,
            Rectangle axisRect,
            PlotDescription plotDescription,
            double animationStartTime,
            double animationEndTime)
        {
            double totalArea = 0;
            var rects = new List<RiemannSumRect>();

            var rectWidth = (riemannSum.EndX - riemannSum.StartX) / riemannSum.NumRects;
            for (int i = 0; i < riemannSum.NumRects; i++)
            {
                var rightX = rectWidth * (i + 1);
                var leftX = rightX - rectWidth;

                var topYStart = riemannSum.FunctionDescription.GetYValue(i % 2 == 1
                    ? rightX
                    : rectWidth * (i + 2));
                var topYEnd = riemannSum.FunctionDescription.GetYValue(rightX);

                totalArea += topYEnd * (rightX - leftX);

                double bottomYStart = 0;
                double bottomYEnd = 0;

                if (topYStart < bottomYStart)
                {
                    var temp = topYStart;
                    topYStart = bottomYStart;
                    bottomYStart = temp;
                }

                if (topYEnd < bottomYEnd)
                {
                    var temp = topYEnd;
                    topYEnd = bottomYEnd;
                    bottomYEnd = temp;
                }

                var visualRightX = (int)GetVisualXValue(rightX, plotDescription.XAxis, axisRect);
                var visualLeftX = (int)GetVisualXValue(leftX, plotDescription.XAxis, axisRect);
                var visualTopYStart = (int)GetVisualYValue(topYStart, plotDescription.YAxis, axisRect);
                var visualTopYEnd = (int)GetVisualYValue(topYEnd, plotDescription.YAxis, axisRect);
                var visualBottomYStart = (int)GetVisualYValue(bottomYStart, plotDescription.YAxis, axisRect);
                var visualBottomYEnd = (int)GetVisualYValue(bottomYEnd, plotDescription.YAxis, axisRect);

                PathDrawing drawing;

                var points = new PointF[]
                    {
                        new PointF(visualLeftX, visualTopYEnd),
                        new PointF(visualRightX, visualTopYEnd),
                        new PointF(visualRightX, visualBottomYEnd),
                        new PointF(visualLeftX, visualBottomYEnd)
                    };

                if (i % 2 == 1)
                {
                    drawing = new PathDrawing(points);
                }
                else
                {
                    var startPoints = new PointF[]
                    {
                        new PointF(visualLeftX, visualTopYStart),
                        new PointF(visualRightX, visualTopYStart),
                        new PointF(visualRightX, visualBottomYStart),
                        new PointF(visualLeftX, visualBottomYStart)
                    };

                    drawing = new PathDrawing(new AnimatedValue<PointF[]>(
                        new ValueAtTime<PointF[]>(startPoints, new AnimationTime(animationStartTime)),
                        new ValueAtTime<PointF[]>(points, new AnimationTime(animationEndTime))));
                }

                drawing.IsClosed = true;
                drawing.HasLockedScale = false;

                rects.Add(new RiemannSumRect(drawing,
                    new RectangleF(visualLeftX, visualTopYEnd, (visualRightX - visualLeftX), (visualBottomYEnd - visualTopYEnd))));
            }

            return new RiemannSumResult(rects, totalArea);
        }

        private class RiemannSumRect
        {
            public readonly PathDrawing Drawing;
            public readonly RectangleF BoundingRect;

            public RiemannSumRect(PathDrawing drawing,
                RectangleF boundingRect)
            {
                Drawing = drawing;
                BoundingRect = boundingRect;
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

        private PathDrawing CreateAreaUnderFunctionDrawing(AreaUnderFunctionDescription areaUnderFunction,
            Rectangle axisRect,
            PlotDescription plotDescription)
        {
            var plotPoints = CreateFunctionDrawingPoints(areaUnderFunction.FunctionDescription,
                axisRect,
                plotDescription,
                areaUnderFunction.StartX,
                areaUnderFunction.EndX);

            var points = new List<PointF>();
            points.Add(CreatePoint(axisRect, plotDescription, areaUnderFunction.StartX, plotDescription.YAxis.MinValue));
            points.AddRange(plotPoints);
            points.Add(CreatePoint(axisRect, plotDescription, areaUnderFunction.EndX, plotDescription.YAxis.MinValue));

            return new PathDrawing(points.ToArray()) { IsClosed = true };
        }

        private PathDrawing CreateFunctionDrawing(IPlottableFunction plottable,
            Rectangle axisRect,
            PlotDescription plotDescription)
        {
            return new PathDrawing(CreateFunctionDrawingPoints(plottable, axisRect, plotDescription, plotDescription.XAxis.MinValue, plotDescription.XAxis.MaxValue).ToArray());
        }

        private IEnumerable<PointF> CreateFunctionDrawingPoints(IPlottableFunction plottable,
            Rectangle axisRect,
            PlotDescription plotDescription,
            double startX,
            double endX)
        {
            var points = new List<PointF>();
            points.Add(CreateFunctionPoint(plottable, axisRect, plotDescription, startX));

            var numTotalPoints = axisRect.Width / 2;
            for (int i = 1; i < numTotalPoints - 1; i++)
                points.Add(CreateFunctionPoint(plottable, axisRect, plotDescription, i * (endX - startX) / numTotalPoints));

            points.Add(CreateFunctionPoint(plottable, axisRect, plotDescription, endX));

            return points;
        }

        private Point CreateFunctionPoint(IPlottableFunction plottable,
            Rectangle axisRect,
            PlotDescription plotDescription,
            double xValue)
        {
            return CreatePoint(axisRect, plotDescription, xValue, plottable.GetYValue(xValue));
        }

        private Point CreatePoint(Rectangle axisRect,
            PlotDescription plotDescription,
            double xValue,
            double yValue)
        {
            var xVisualValue = GetVisualXValue(xValue, plotDescription.XAxis, axisRect);
            var yVisualValue = GetVisualYValue(yValue, plotDescription.YAxis, axisRect);

            return new Point((int)Math.Round(xVisualValue), (int)Math.Round(yVisualValue));
        }

        private double GetVisualXValue(double value,
            AxisRangeDescription axisRangeDescription,
            Rectangle axisRect)
        {
            var percentage = GetPercentage(value, axisRangeDescription);
            return axisRect.Left + (percentage * axisRect.Width);
        }

        private double GetVisualYValue(double value,
            AxisRangeDescription axisRangeDescription,
            Rectangle axisRect)
        {
            var percentage = GetPercentage(value, axisRangeDescription);
            return axisRect.Bottom - (percentage * axisRect.Height);
        }

        private double GetPercentage(double value,
            AxisRangeDescription axis)
        {
            return (value - axis.MinValue) / (axis.MaxValue - axis.MinValue);
        }
    }
}
