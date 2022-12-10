using RenderingDescriptions;
using RenderingDescriptions.How;
using RenderingDescriptions.What;
using RenderingDescriptions.When;
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
    }

    public class PlotAxisLayoutDescription
    {
        public readonly double Length;
        public readonly double StartValue;
        public readonly double EndValue;

        public PlotAxisLayoutDescription(double length,
            double startValue,
            double endValue)
        {
            Length = length;
            StartValue = startValue;
            EndValue = endValue;
        }
    }
}
