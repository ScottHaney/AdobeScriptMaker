﻿using AdobeComponents.Animation;
using AdobeComponents.Components;
using RenderingDescriptions.How;
using RenderingDescriptions.When;
using MathRenderingDescriptions.Plot.What;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace MathRenderingDescriptions.Plot.How
{
    public class AxesRenderer : IHowToRender
    {
        private readonly AxesRenderingDescription _description;
        private readonly AbsoluteTiming _drawingDuration;

        public AbsoluteTiming EntranceAnimationStart { get; set; } = new AbsoluteTiming(0);
        public AbsoluteTiming EntranceAnimationEnd { get; set; } = new AbsoluteTiming(0);

        public AxesRenderer(AxesRenderingDescription description,
            AbsoluteTiming drawingDuration)
        {
            _description = description;
            _drawingDuration = drawingDuration;
        }

        public RenderedComponents Render(AbsoluteTiming whenToRender)
        {
            var xAxisPoints = _description.PlotLayoutDescription.GetXAxisPoints();
            var yAxisPoints = _description.PlotLayoutDescription.GetYAxisPoints();
            var originPoint = _description.PlotLayoutDescription.GetAxesIntersectionPoint();

            var xAxisValues = new AnimatedValue<PointF[]>(
                new ValueAtTime<PointF[]>(new PointF[] { originPoint, originPoint }, new AnimationTime(EntranceAnimationStart.Time)),
                new ValueAtTime<PointF[]>(xAxisPoints, new AnimationTime(EntranceAnimationEnd.Time)));

            var yAxisValues = new AnimatedValue<PointF[]>(
                new ValueAtTime<PointF[]>(new PointF[] { originPoint, originPoint }, new AnimationTime(EntranceAnimationStart.Time)),
                new ValueAtTime<PointF[]>(yAxisPoints, new AnimationTime(EntranceAnimationEnd.Time)));

            var xAxis = new AdobePathComponent(xAxisValues);
            var yAxis = new AdobePathComponent(yAxisValues);

            return new RenderedComponents(new IAdobeLayerComponent[] { xAxis, yAxis }
                .Select(x => new TimedAdobeLayerComponent(x, whenToRender.Time + EntranceAnimationStart.Time, whenToRender.Time + EntranceAnimationStart.Time + _drawingDuration.Time))
                .ToArray());
        }
    }
}
