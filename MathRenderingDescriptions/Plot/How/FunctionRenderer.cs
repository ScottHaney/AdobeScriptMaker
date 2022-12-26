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
                    TrimPaths = new AdobeComponents.Effects.AdobeTrimPathsEffect()
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
