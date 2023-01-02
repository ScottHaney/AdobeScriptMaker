using AdobeComponents.Animation;
using AdobeComponents.Components;
using RenderingDescriptions.How;
using RenderingDescriptions.When;
using MathRenderingDescriptions.Plot.What;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using RenderingDescriptions.Timing;

namespace MathRenderingDescriptions.Plot.How
{
    public class FunctionRenderer : IHowToRender
    {
        private readonly FunctionRenderingDescription _description;
        private readonly FunctionPointsRenderer _pointsRenderer;

        public FunctionRenderer(FunctionRenderingDescription description,
            FunctionPointsRenderer pointsRenderer)
        {
            _description = description;
            _pointsRenderer = pointsRenderer;
        }

        public RenderedComponents Render(ITimingForRender timing)
        {
            var points = _pointsRenderer.GetPoints();

            return new RenderedComponents(new TimedAdobeLayerComponent(
                new AdobePathGroupComponent(new AdobePathComponent(new StaticValue<PointF[]>(points))
                {
                    TrimPathsEffect = new AdobeComponents.Effects.AdobeTrimPathsEffect()
                    {
                        Start = new StaticValue<double>(0),
                        End = new AnimatedValue<double>(
                            new ValueAtTime<double>(0, new AnimationTime(timing.WhenToStart.Time)),
                            new ValueAtTime<double>(100, new AnimationTime(timing.WhenToStart.Time + timing.EntranceAnimationDuration.Time)))
                    }
                }),
                timing.WhenToStart.Time,
                timing.WhenToStart.Time + timing.RenderDuration.Time));
        }
    }

    public class FunctionPointsRenderer
    {
        private readonly FunctionRenderingDescription _description;

        public int PixelStep { get; set; } = 2;

        public FunctionPointsRenderer(FunctionRenderingDescription description)
        {
            _description = description;
        }

        public PointF[] GetPoints()
        {
            var visualStartX = _description.PlotLayoutDescription.GetVisualXValue(_description.StartX);
            var visualEndX = _description.PlotLayoutDescription.GetVisualXValue(_description.EndX);

            var numPixels = (int)(visualEndX - visualStartX + 1);
            var numPixelSteps = numPixels / PixelStep;

            var visualStep = (_description.EndX - _description.StartX) / numPixelSteps;

            var points = new List<PointF>();
            points.Add(_description.PlotLayoutDescription.CreateFunctionPoint(_description, _description.StartX));

            for (double i = _description.StartX + visualStep; i < _description.EndX; i += visualStep)
            {
                points.Add(_description.PlotLayoutDescription.CreateFunctionPoint(_description, i));
            }

            points.Add(_description.PlotLayoutDescription.CreateFunctionPoint(_description, _description.EndX));

            return points.ToArray();
        }
    }
}
