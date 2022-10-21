using DirectRendering.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DirectRendering.Plotting
{
    public class PlotAxes : CompositeDrawing
    {
        private readonly PathDrawing XAxis;
        private readonly PathDrawing YAxis;

        public PlotAxes(Rectangle bounds)
            : this(CreateAxes(bounds))
        { }

        private PlotAxes(PathDrawing[] axes)
            : base(axes)
        {
            XAxis = axes[0];
            YAxis = axes[1];
        }

        private static PathDrawing[] CreateAxes(Rectangle bounds)
        {
            return new[]
            {
                new PathDrawing(new Point(bounds.Left, bounds.Bottom), new Point(bounds.Left, bounds.Top)),
                new PathDrawing(new Point(bounds.Left, bounds.Bottom), new Point(bounds.Right, bounds.Bottom))
            };
        }
    }
}
