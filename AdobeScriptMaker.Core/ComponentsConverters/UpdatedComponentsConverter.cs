﻿using RenderingDescriptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RenderingDescriptions.When;
using MathRenderingDescriptions.Plot.What;
using MathRenderingDescriptions.Plot.How;
using RenderingDescriptions.How;
using AdobeComponents.Components;
using AdobeComponents.Components.Layers;
using MathRenderingDescriptions.Plot.How.RiemannSums;
using MathRenderingDescriptions.Plot.What.RiemannSums;
using RenderingDescriptions.Timing;

namespace AdobeScriptMaker.Core.ComponentsConverters
{
    public class UpdatedComponentsConverter
    {
        public AdobeScript Convert(List<RenderingDescription> renderingDescriptions)
        {
            var layers = new List<AdobeLayer>();

            foreach (var timedComponent in CreateComponents(renderingDescriptions))
            {
                var components = timedComponent.Component is GroupedTogetherAdobeLayerComponents group
                    ? group.Components
                    : new IAdobeLayerComponent[] { timedComponent.Component };

                var layer = new AdobeLayer(components)
                    {
                        InPoint = timedComponent.StartTime,
                        OutPoint = timedComponent.EndTime
                    };

                layers.Add(layer);
            }

            var defaultComp = new AdobeComposition(layers.ToArray());
            return new AdobeScript(defaultComp);
        }

        public IEnumerable<TimedAdobeLayerComponent> CreateComponents(List<RenderingDescription> renderingDescriptions)
        {
            var results = new List<RenderedComponents>();
            foreach (var renderingDescription in renderingDescriptions)
            {
                if (renderingDescription.What is AxesRenderingDescription axes)
                {
                    results.Add(RenderAxes(axes, renderingDescription.Timing));
                }
                else if (renderingDescription.What is FunctionRenderingDescription function)
                {
                    results.Add(RenderFunction(function, renderingDescription.Timing));
                }
                else if (renderingDescription.What is AreaUnderFunctionRenderingDescription auf)
                {
                    results.Add(RenderAreaUnderFunction(auf, renderingDescription.Timing));
                }
                else if (renderingDescription.What is RiemannSumsRenderingDescription rs)
                {
                    results.Add(RenderRiemannSums(rs, renderingDescription.Timing));
                }
                else if (renderingDescription.What is DataTableRenderingDescription dt)
                {
                    results.Add(RenderDataTable(dt, renderingDescription.Timing));
                }
            }

            return results.SelectMany(x => x.Components);
        }

        private RenderedComponents RenderAxes(AxesRenderingDescription axes,
            ITimingForRender timing)
        {
            var axesRenderer = new AxesRenderer(axes);
            return axesRenderer.Render(timing);
        }

        private RenderedComponents RenderFunction(FunctionRenderingDescription function,
            ITimingForRender timing)
        {
            var axesRenderer = new FunctionRenderer(function,
                CreatePointsRenderer(function));

            return axesRenderer.Render(timing);
        }

        private RenderedComponents RenderAreaUnderFunction(AreaUnderFunctionRenderingDescription auf,
            ITimingForRender timing)
        {
            var renderer = new AreaUnderFunctionRenderer(auf,
                CreatePointsRenderer(auf.FunctionRenderingDescription));

            return renderer.Render(timing);
        }

        private RenderedComponents RenderRiemannSums(RiemannSumsRenderingDescription rs,
            ITimingForRender timing)
        {
            var renderer = new RiemannSumsRenderer(rs, new RiemannSumsTiming(rs));
            return renderer.Render(timing);
        }

        private RenderedComponents RenderDataTable(DataTableRenderingDescription dt,
            ITimingForRender timing)
        {
            var renderer = new DataTableRenderer(dt);
            return renderer.Render(timing);
        }

        private FunctionPointsRenderer CreatePointsRenderer(FunctionRenderingDescription function)
        {
            return new FunctionPointsRenderer(function);
        }
    }

    public class HowToRenderResult
    {
        public readonly IEnumerable<TimedAdobeLayerComponent> Components;
        public readonly IHowToRender How;

        public HowToRenderResult(RenderedComponents renderedComponents,
            IHowToRender how)
        {
            Components = renderedComponents.Components;
            How = how;
        }
    }
}
