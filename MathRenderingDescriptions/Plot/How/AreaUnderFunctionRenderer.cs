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

namespace MathRenderingDescriptions.Plot.How
{
    public class AreaUnderFunctionRenderer : IHowToRender
    {
        private readonly AreaUnderFunctionRenderingDescription _description;
        private readonly FunctionPointsRenderer _functionRenderer;
        private readonly AbsoluteTiming _drawingDuration;

        public AreaUnderFunctionRenderer(AreaUnderFunctionRenderingDescription description,
            FunctionPointsRenderer functionRenderer,
            AbsoluteTiming drawingDuration)
        {
            _description = description;
            _functionRenderer = functionRenderer;
            _drawingDuration = drawingDuration;
        }

        public RenderedComponents Render(AbsoluteTiming whenToRender)
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

            return new RenderedComponents(new TimedAdobeLayerComponent(
                new AdobePathComponent(new StaticValue<PointF[]>(areaUnderFunctionPoints)) { IsClosed = true },
                whenToRender.Time,
                _drawingDuration.Time));
        }
    }
}
