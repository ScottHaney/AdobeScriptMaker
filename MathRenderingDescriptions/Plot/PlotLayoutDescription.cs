using RenderingDescriptions;
using RenderingDescriptions.How;
using RenderingDescriptions.What;
using RenderingDescriptions.When;
using MathRenderingDescriptions.Plot.What;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MathRenderingDescriptions.Plot
{
    public class PlotLayoutDescription
    {
        public readonly PlotAxesLayoutDescription AxesLayout;
        public readonly PointF TopLeft;
        public PlotLayoutDescription(PlotAxesLayoutDescription axesLayout,
            PointF topLeft)
        {
            AxesLayout = axesLayout;
            TopLeft = topLeft;
        }

        public Point CreateFunctionPoint(FunctionRenderingDescription function,
            double xValue)
        {
            return AxesLayout.CreateFunctionPoint(function, GetBounds(), xValue);
        }

        public RectangleF GetBounds()
        {
            return new RectangleF(TopLeft,
                new SizeF((float)AxesLayout.XAxis.Length, (float)AxesLayout.YAxis.Length));
        }

        public PointF[] GetXAxisPoints()
        {
            var bounds = GetBounds();

            return new PointF[]
            {
                new PointF(bounds.Left, bounds.Bottom),
                new PointF(bounds.Right, bounds.Bottom)
            };
        }

        public PointF[] GetYAxisPoints()
        {
            var bounds = GetBounds();

            return new PointF[]
            {
                new PointF(bounds.Left, bounds.Bottom),
                new PointF(bounds.Left, bounds.Top)
            };
        }

        public PointF GetAxesIntersectionPoint()
        {
            var bounds = GetBounds();
            return new PointF(bounds.Left, bounds.Bottom);
        }
    }

    public class PlotAxesLayoutDescription
    {
        public readonly PlotAxisLayoutDescription XAxis;
        public readonly PlotAxisLayoutDescription YAxis;

        public PlotAxesLayoutDescription(PlotAxisLayoutDescription xAxis,
            PlotAxisLayoutDescription yAxis)
        {
            XAxis = xAxis;
            YAxis = yAxis;
        }

        public Point CreateFunctionPoint(FunctionRenderingDescription function,
            RectangleF axisRect,
            double xValue)
        {
            return CreatePoint(axisRect, xValue, function.Function(xValue));
        }

        private Point CreatePoint(RectangleF axisBoundingBox,
            double xValue,
            double yValue)
        {
            var xVisualValue = GetVisualXValue(xValue, XAxis, axisBoundingBox);
            var yVisualValue = GetVisualYValue(yValue, YAxis, axisBoundingBox);

            return new Point((int)Math.Round(xVisualValue), (int)Math.Round(yVisualValue));
        }

        private double GetVisualXValue(double value,
            PlotAxisLayoutDescription axis,
            RectangleF axisBoundingBox)
        {
            var percentage = GetPercentage(value, axis);
            return axisBoundingBox.Left + (percentage * axisBoundingBox.Width);
        }

        private double GetVisualYValue(double value,
            PlotAxisLayoutDescription axis,
            RectangleF axisBoundingBox)
        {
            var percentage = GetPercentage(value, axis);
            return axisBoundingBox.Bottom - (percentage * axisBoundingBox.Height);
        }

        private double GetPercentage(double value,
            PlotAxisLayoutDescription axis)
        {
            return (value - axis.MinValue) / (axis.MaxValue - axis.MinValue);
        }
    }

    public class PlotAxisLayoutDescription
    {
        public readonly double Length;
        public readonly double MinValue;
        public readonly double MaxValue;

        public PlotAxisLayoutDescription(double length,
            double minValue,
            double maxValue)
        {
            Length = length;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
