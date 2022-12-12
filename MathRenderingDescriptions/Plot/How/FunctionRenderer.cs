using AdobeComponents.Animation;
using AdobeComponents.Components;
using RenderingDescriptions.How;
using RenderingDescriptions.When;
using MathRenderingDescriptions.Plot.What;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MathRenderingDescriptions.Plot.How
{
    public class FunctionRenderer : IHowToRender
    {
        private readonly FunctionRenderingDescription _description;
        private readonly FunctionPointsRenderer _pointsRenderer;
        private readonly AbsoluteTiming _drawingDuration;

        public FunctionRenderer(FunctionRenderingDescription description,
            FunctionPointsRenderer pointsRenderer,
            AbsoluteTiming drawingDuration)
        {
            _description = description;
            _pointsRenderer = pointsRenderer;
            _drawingDuration = drawingDuration;
        }

        public RenderedComponents Render(AbsoluteTiming whenToRender)
        {
            var points = _pointsRenderer.GetPoints();

            return new RenderedComponents(new TimedAdobeLayerComponent(
                new AdobePathComponent(new StaticValue<PointF[]>(points)),
                whenToRender.Time,
                _drawingDuration.Time));
        }
    }

    public class FunctionPointsRenderer
    {
        private readonly FunctionRenderingDescription _description;

        public FunctionPointsRenderer(FunctionRenderingDescription description)
        {
            _description = description;
        }

        public PointF[] GetPoints()
        {
            return new PointF[]
            {
                _description.PlotLayoutDescription.CreateFunctionPoint(_description, _description.StartX),
                _description.PlotLayoutDescription.CreateFunctionPoint(_description, _description.EndX)
            };
        }
    }
}
