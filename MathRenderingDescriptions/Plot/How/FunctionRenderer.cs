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
        private readonly double _startX;
        private readonly double _endX;

        public FunctionPointsRenderer(FunctionRenderingDescription description,
            double startX,
            double endX)
        {
            _description = description;
            _startX = startX;
            _endX = endX;
        }

        public PointF[] GetPoints()
        {
            return new PointF[]
            {
                _description.PlotLayoutDescription.CreateFunctionPoint(_description, _startX),
                _description.PlotLayoutDescription.CreateFunctionPoint(_description, _endX)
            };
        }
    }
}
