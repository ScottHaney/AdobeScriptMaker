using MathDescriptions.Plot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DirectRendering.Drawing;

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

            foreach (var drawing in axes.GetDrawings())
                yield return drawing;
        }

        private PathDrawing CreateFunctionDrawing(IPlottable plottable,
            Rectangle axisRect,
            PlotDescription plotDescription)
        {
            var points = new List<Point>();
            points.Add(CreateFunctionPoint(plottable, axisRect, plotDescription, plotDescription.XAxis.MinValue));

            var numTotalPoints = axisRect.Width / 2;
            for (int i = 1; i < numTotalPoints - 1; i++)
                points.Add(CreateFunctionPoint(plottable, axisRect, plotDescription, i * (plotDescription.XAxis.MaxValue - plotDescription.XAxis.MinValue) / numTotalPoints));

            points.Add(CreateFunctionPoint(plottable, axisRect, plotDescription, plotDescription.XAxis.MaxValue));

            return new PathDrawing(points.ToArray());
        }

        private Point CreateFunctionPoint(IPlottable plottable,
            Rectangle axisRect,
            PlotDescription plotDescription,
            double xValue)
        {
            var yValue = plottable.GetYValue(xValue);

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
