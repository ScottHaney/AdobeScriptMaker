using AdobeComponents.Animation;
using AdobeComponents.Components;
using RenderingDescriptions.How;
using RenderingDescriptions.When;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MathRenderingDescriptions.Plot.How
{
    public class FunctionRenderer : IHowToRender
    {
        private readonly FunctionRenderingDescription _description;
        private readonly AbsoluteTiming _drawingDuration;
        private readonly double _startX;
        private readonly double _endX;

        public FunctionRenderer(FunctionRenderingDescription description,
            AbsoluteTiming drawingDuration,
            double startX,
            double endX)
        {
            _description = description;
            _drawingDuration = drawingDuration;
            _startX = startX;
            _endX = endX;
        }

        public HowToRenderResult Render()
        {
            var points = new PointF[]
            {
                _description.PlotLayoutDescription.CreateFunctionPoint(_description, _startX),
                _description.PlotLayoutDescription.CreateFunctionPoint(_description, _endX)
            };

            return new HowToRenderResult(_drawingDuration.Time,
                new AdobePathComponent(new StaticValue<PointF[]>(points)));
        }
    }
}
