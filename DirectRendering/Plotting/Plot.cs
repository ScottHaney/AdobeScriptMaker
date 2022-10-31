using MathDescriptions.Plot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DirectRendering.Drawing;
using MathDescriptions.Plot.Functions;
using System.Linq;
using MathDescriptions.Plot.Calculus;

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
                foreach (var path in CreateRiemannSum(decorator, _visualBounds, _plotDescription))
                    yield return path;
            }

            foreach (var drawing in axes.GetDrawings())
                yield return drawing;
        }

        private IEnumerable<PathDrawing> CreateRiemannSum(RiemannSumDescription riemannSum,
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

                var points = new PointF[]
                {
                    new PointF(visualLeftX, visualTopY),
                    new PointF(visualRightX, visualTopY),
                    new PointF(visualRightX, visualBottomY),
                    new PointF(visualLeftX, visualBottomY)
                };

                yield return new PathDrawing(points) { IsClosed = true, HasLockedScale = false };
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
