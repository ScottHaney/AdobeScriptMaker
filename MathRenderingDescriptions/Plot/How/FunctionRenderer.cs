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
        private readonly IFunctionPointsRenderer _pointsRenderer;

        public FunctionRenderer(IFunctionPointsRenderer pointsRenderer)
        {
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

    public interface IFunctionPointsRenderer
    {
        PointF[] GetPoints();
    }

    public class FunctionPointsRenderer : IFunctionPointsRenderer
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

    public class PolarFunctionPointsRenderer : IFunctionPointsRenderer
    {
        private readonly PolarFunctionRenderingDescription _description;

        public double AngleStep { get; set; } = 0.1;

        public PolarFunctionPointsRenderer(PolarFunctionRenderingDescription description)
        {
            _description = description;
        }

        public PointF[] GetPoints()
        {
            var numRadians = _description.EndAngle - _description.StartAngle;

            var points = new List<PointF>();
            points.Add(_description.PlotLayoutDescription.CreateFunctionPoint(_description, _description.StartAngle));

            for (double i = _description.StartAngle + AngleStep; i < _description.EndAngle; i += AngleStep)
            {
                points.Add(_description.PlotLayoutDescription.CreateFunctionPoint(_description, i));
            }

            points.Add(_description.PlotLayoutDescription.CreateFunctionPoint(_description, _description.EndAngle));

            return points.ToArray();
        }
    }
}
