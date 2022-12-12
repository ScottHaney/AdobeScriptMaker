using RenderingDescriptions;
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

namespace AdobeScriptMaker.Core.ComponentsConverters
{
    public class UpdatedComponentsConverter
    {
        public AdobeScript Convert(List<RenderingDescription> renderingDescriptions,
            AbsoluteTiming maxDuration)
        {
            var layers = new List<AdobeLayer>();

            foreach (var timedComponent in CreateComponents(renderingDescriptions, maxDuration))
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

        public IEnumerable<TimedAdobeLayerComponent> CreateComponents(List<RenderingDescription> renderingDescriptions,
            AbsoluteTiming maxDuration)
        {
            var results = new List<RenderedComponents>();
            foreach (var renderingDescription in renderingDescriptions)
            {
                if (renderingDescription.What is AxesRenderingDescription axes)
                {
                    results.Add(RenderAxes(axes, maxDuration, renderingDescription.WhenToStart));
                }
                else if (renderingDescription.What is FunctionRenderingDescription function)
                {
                    results.Add(RenderFunction(function, maxDuration, renderingDescription.WhenToStart));
                }
                else if (renderingDescription.What is AreaUnderFunctionRenderingDescription auf)
                {
                    results.Add(RenderAreaUnderFunction(auf, maxDuration, renderingDescription.WhenToStart));
                }
                else if (renderingDescription.What is RiemannSumsRenderingDescription rs)
                {
                    results.Add(RenderRiemannSums(rs, maxDuration, renderingDescription.WhenToStart));
                }
            }

            return results.SelectMany(x => x.Components);

        }

        private RenderedComponents RenderAxes(AxesRenderingDescription axes,
            AbsoluteTiming maxDuration,
            AbsoluteTiming whenToStart)
        {
            var axesRenderer = new AxesRenderer(axes, maxDuration)
            {
                EntranceAnimationEnd = new AbsoluteTiming(2)
            };

            return axesRenderer.Render(whenToStart);
        }

        private RenderedComponents RenderFunction(FunctionRenderingDescription function,
            AbsoluteTiming maxDuration,
            AbsoluteTiming whenToStart)
        {
            var axesRenderer = new FunctionRenderer(function,
                CreatePointsRenderer(function),
                maxDuration);

            return axesRenderer.Render(whenToStart);
        }

        private RenderedComponents RenderAreaUnderFunction(AreaUnderFunctionRenderingDescription auf,
            AbsoluteTiming maxDuration,
            AbsoluteTiming whenToStart)
        {
            var renderer = new AreaUnderFunctionRenderer(auf,
                CreatePointsRenderer(auf.FunctionRenderingDescription),
                maxDuration);

            return renderer.Render(whenToStart);
        }

        private RenderedComponents RenderRiemannSums(RiemannSumsRenderingDescription rs,
            AbsoluteTiming maxDuration,
            AbsoluteTiming whenToStart)
        {
            var renderer = new RiemannSumsRenderer(rs);
            return renderer.Render(whenToStart);
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
