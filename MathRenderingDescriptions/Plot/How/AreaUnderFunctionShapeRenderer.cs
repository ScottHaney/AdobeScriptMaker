using MathRenderingDescriptions.Plot.What;
using RenderingDescriptions.How;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;
using RenderingDescriptions.When;
using AdobeComponents.Components;
using AdobeComponents.Animation;
using AdobeComponents.Effects;
using AdobeComponents.CommonValues;
using RenderingDescriptions.Timing;

namespace MathRenderingDescriptions.Plot.How
{
    public class AreaUnderFunctionShapeRenderer : IHowToRender
    {
        private readonly AreaUnderFunctionShapeRenderingDescription _description;
        private readonly FunctionPointsRenderer _functionRenderer;

        public AreaUnderFunctionShapeRenderer(AreaUnderFunctionShapeRenderingDescription description,
            FunctionPointsRenderer functionRenderer)
        {
            _description = description;
            _functionRenderer = functionRenderer;
        }

        public RenderedComponents Render(ITimingForRender timing)
        {
            var functionPoints = _functionRenderer.GetPoints();

            var firstPoint = functionPoints.First();
            var lastPoint = functionPoints.Last();

            var plotLayoutDescription = _description.FunctionRenderingDescription.PlotLayoutDescription;

            var additionalPoints = new PointF[]
            {
                new PointF(lastPoint.X, plotLayoutDescription.GetAxesIntersectionPoint().Y),
                new PointF(firstPoint.X, plotLayoutDescription.GetAxesIntersectionPoint().Y)
            };

            var areaUnderFunctionPoints = functionPoints.Concat(additionalPoints).ToArray();

            var path = new AdobePathComponent(new StaticValue<PointF[]>(areaUnderFunctionPoints)) { IsClosed = true };

            var trimPaths = new AdobeTrimPathsEffect()
            {
                End = new AnimatedValue<double>(new ValueAtTime<double>(0, new AnimationTime(timing.WhenToStart.Time)),
                    new ValueAtTime<double>(100, new AnimationTime(timing.WhenToStart.Time + timing.EntranceAnimationDuration.Time))),
                Start = new AnimatedValue<double>(new ValueAtTime<double>(0, new AnimationTime(timing.WhenToStart.Time + timing.RenderDuration.Time - timing.ExitAnimationDuration.Time)),
                    new ValueAtTime<double>(100, new AnimationTime(timing.WhenToStart.Time + timing.RenderDuration.Time)))
            };
            path.TrimPathsEffect = trimPaths;

            return new RenderedComponents(
                new TimedAdobeLayerComponent(
                    new GroupedTogetherAdobeLayerComponents(
                        new AdobePathGroupComponent(path)),
                        timing.WhenToStart.Time,
                        timing.WhenToStart.Time + timing.RenderDuration.Time));
        }
    }
}
