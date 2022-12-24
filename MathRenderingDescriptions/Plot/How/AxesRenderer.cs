using AdobeComponents.Animation;
using AdobeComponents.Components;
using RenderingDescriptions.How;
using RenderingDescriptions.When;
using MathRenderingDescriptions.Plot.What;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using RenderingDescriptions.Timing;

namespace MathRenderingDescriptions.Plot.How
{
    public class AxesRenderer : IHowToRender
    {
        private readonly AxesRenderingDescription _description;

        public AxesRenderer(AxesRenderingDescription description)
        {
            _description = description;
        }

        public RenderedComponents Render(ITimingForRender timing)
        {
            var xAxisPoints = _description.PlotLayoutDescription.GetXAxisPoints();
            var yAxisPoints = _description.PlotLayoutDescription.GetYAxisPoints();
            var originPoint = _description.PlotLayoutDescription.GetAxesIntersectionPoint();

            var animationStartTime = new AnimationTime(timing.WhenToStart.Time);
            var animationEndTime = new AnimationTime(timing.WhenToStart.Time + timing.EntranceAnimationDuration.Time);

            var xAxisValues = new AnimatedValue<PointF[]>(
                new ValueAtTime<PointF[]>(new PointF[] { originPoint, originPoint }, animationStartTime),
                new ValueAtTime<PointF[]>(xAxisPoints, animationEndTime));

            var yAxisValues = new AnimatedValue<PointF[]>(
                new ValueAtTime<PointF[]>(new PointF[] { originPoint, originPoint }, animationStartTime),
                new ValueAtTime<PointF[]>(yAxisPoints, animationEndTime));

            var xAxis = new AdobePathComponent(xAxisValues);
            var yAxis = new AdobePathComponent(yAxisValues);

            return new RenderedComponents(new IAdobeLayerComponent[] { xAxis, yAxis }
                .Select(x => new TimedAdobeLayerComponent(x, timing.WhenToStart.Time, timing.WhenToStart.Time + timing.RenderDuration.Time))
                .ToArray());
        }
    }
}
