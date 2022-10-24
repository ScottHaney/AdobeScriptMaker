using MathDescriptions.Plot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DirectRendering.Drawing;
using MathDescriptions.Plot.Functions;
using System.Linq;

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

            foreach (var drawing in axes.GetDrawings())
                yield return drawing;
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

            var points = new List<Point>();
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

        private IEnumerable<Point> CreateFunctionDrawingPoints(IPlottableFunction plottable,
            Rectangle axisRect,
            PlotDescription plotDescription,
            double startX,
            double endX)
        {
            var points = new List<Point>();
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
            var xAxisPercentage = GetPercentage(xValue, plotDescription.XAxis);
            var yAxisPercentage = GetPercentage(yValue, plotDescription.YAxis);

            var xVisualValue = GetVisualXValue(xAxisPercentage, axisRect);
            var yVisualValue = GetVisualYValue(yAxisPercentage, axisRect);

            return new Point((int)Math.Round(xVisualValue), (int)Math.Round(yVisualValue));
        }

        private double GetVisualXValue(double percentage,
            Rectangle axisRect)
        {
            return axisRect.Left + (percentage * axisRect.Width);
        }

        private double GetVisualYValue(double percentage,
            Rectangle axisRect)
        {
            return axisRect.Bottom - (percentage * axisRect.Height);
        }

        private double GetPercentage(double value,
            AxisRangeDescription axis)
        {
            return (value - axis.MinValue) / (axis.MaxValue - axis.MinValue);
        }
    }
}
