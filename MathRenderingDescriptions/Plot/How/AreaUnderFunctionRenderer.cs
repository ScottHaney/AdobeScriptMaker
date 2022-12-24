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
    public class AreaUnderFunctionRenderer : IHowToRender
    {
        private readonly AreaUnderFunctionRenderingDescription _description;
        private readonly FunctionPointsRenderer _functionRenderer;

        public AreaUnderFunctionRenderer(AreaUnderFunctionRenderingDescription description,
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
            var mask = new AdobeMaskComponent(path) { MaskName = "AreaUnderFunctionMask" };
            var scribble = new AdobeScribbleEffect(mask.MaskName)
            {
                ColorValue = new AdobeColorValue("[0, 0, 0]"),
                End = new AnimatedValue<double>(new ValueAtTime<double>(0, new AnimationTime(timing.WhenToStart.Time)),
                    new ValueAtTime<double>(100, new AnimationTime(timing.WhenToStart.Time + timing.EntranceAnimationDuration.Time)))
            };

            return new RenderedComponents(
                new TimedAdobeLayerComponent(
                    new GroupedTogetherAdobeLayerComponents(
                        path, mask, scribble),
                        timing.WhenToStart.Time,
                        timing.WhenToStart.Time + timing.RenderDuration.Time));
        }
    }
}
