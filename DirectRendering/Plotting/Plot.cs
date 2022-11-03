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
                foreach (var path in CreateRiemannSum_BottomUp(decorator, _visualBounds, _plotDescription))
                    yield return path.Drawing;
            }

            foreach (var decorator in _plotDescription.Decorations.OfType<RiemannSumsDescription>())
            {
                foreach (var path in CreateRiemannSums(decorator, _visualBounds, _plotDescription))
                    yield return path;
            }

            foreach (var drawing in axes.GetDrawings())
                yield return drawing;
        }

        private IEnumerable<IDrawing> CreateRiemannSums(RiemannSumsDescription riemannSums,
            Rectangle axisRect,
            PlotDescription plotDescription)
        {
            var currentSumDescription = riemannSums.RiemannSumStart;
            var currentTime = 0;
            for (int i = 1; i <= riemannSums.NumSums; i++)
            {
                IEnumerable<RiemannSumResult> currentRiemannSum;
                if (i == 1)
                    currentRiemannSum = CreateRiemannSum_BottomUp(currentSumDescription, axisRect, plotDescription);
                else
                    currentRiemannSum = CreateRiemannSum_SplitTopDown(currentSumDescription, axisRect, plotDescription, currentTime + 0.5);

                yield return new TimingContext(new AbsoluteTimingContextTime(currentTime),
                    new AbsoluteTimingContextTime(riemannSums.NumSums == i ? 30 : 4),
                    currentRiemannSum.Select(x => x.Drawing).ToArray());

                if (riemannSums.NumSums != i)
                {
                    var lines = currentRiemannSum.Select(x => CreateSplitLine(x, currentTime + 3, currentTime + 4)).ToArray();
                    yield return new TimingContext(new AbsoluteTimingContextTime(currentTime + 3),
                        new AbsoluteTimingContextTime(1),
                        lines);
                }

                var nextRiemannSum = new RiemannSumDescription(riemannSums.RiemannSumStart.FunctionDescription,
                    currentSumDescription.NumRects * 2,
                    riemannSums.RiemannSumStart.StartX,
                    riemannSums.RiemannSumStart.EndX);

                currentSumDescription = nextRiemannSum;
                currentTime += 4;
            }
        }

        private PathDrawing CreateSplitLine(RiemannSumResult RiemannSumRect,
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

        private IEnumerable<RiemannSumResult> CreateRiemannSum_BottomUp(RiemannSumDescription riemannSum,
            Rectangle axisRect,
            PlotDescription plotDescription)
        {
            var rectWidth = (riemannSum.EndX - riemannSum.StartX) / riemannSum.NumRects;
            for (int i = 0; i < riemannSum.NumRects; i++)
            {
                var rightX = rectWidth * (i + 1);
                var leftX = rightX - rectWidth;
                var topY = riemannSum.FunctionDescription.GetYValue(rightX);
                var bottomY = plotDescription.YAxis.MinValue;

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

                yield return new RiemannSumResult(drawing, new RectangleF(visualLeftX, visualTopY, (visualRightX - visualLeftX), (visualBottomY - visualTopY))); ;
            }
        }

        private IEnumerable<RiemannSumResult> CreateRiemannSum_SplitTopDown(RiemannSumDescription riemannSum,
            Rectangle axisRect,
            PlotDescription plotDescription,
            double animationStartTime)
        {
            var rectWidth = (riemannSum.EndX - riemannSum.StartX) / riemannSum.NumRects;
            for (int i = 0; i < riemannSum.NumRects; i++)
            {
                var rightX = rectWidth * (i + 1);
                var leftX = rightX - rectWidth;

                var topYStart = riemannSum.FunctionDescription.GetYValue(i % 2 == 1
                    ? rightX
                    : rectWidth * (i + 2));
                var topYEnd = riemannSum.FunctionDescription.GetYValue(rightX);

                var bottomY = plotDescription.YAxis.MinValue;

                var visualRightX = (int)GetVisualXValue(rightX, plotDescription.XAxis, axisRect);
                var visualLeftX = (int)GetVisualXValue(leftX, plotDescription.XAxis, axisRect);
                var visualTopYStart = (int)GetVisualYValue(topYStart, plotDescription.YAxis, axisRect);
                var visualTopYEnd = (int)GetVisualYValue(topYEnd, plotDescription.YAxis, axisRect);
                var visualBottomY = (int)GetVisualYValue(bottomY, plotDescription.YAxis, axisRect);

                PathDrawing drawing;

                var points = new PointF[]
                    {
                        new PointF(visualLeftX, visualTopYEnd),
                        new PointF(visualRightX, visualTopYEnd),
                        new PointF(visualRightX, visualBottomY),
                        new PointF(visualLeftX, visualBottomY)
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
                        new PointF(visualRightX, visualBottomY),
                        new PointF(visualLeftX, visualBottomY)
                    };

                    drawing = new PathDrawing(new AnimatedValue<PointF[]>(
                        new ValueAtTime<PointF[]>(startPoints, new AnimationTime(animationStartTime)),
                        new ValueAtTime<PointF[]>(points, new AnimationTime(animationStartTime + 1))));
                }

                drawing.IsClosed = true;
                drawing.HasLockedScale = false;

                yield return new RiemannSumResult(drawing, new RectangleF(visualLeftX, visualTopYEnd, (visualRightX - visualLeftX), (visualBottomY - visualTopYEnd))); ;
            }
        }

        private class RiemannSumResult
        {
            public readonly PathDrawing Drawing;
            public readonly RectangleF BoundingRect;

            public RiemannSumResult(PathDrawing drawing,
                RectangleF boundingRect)
            {
                Drawing = drawing;
                BoundingRect = boundingRect;
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
