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
                var layer = new AdobeLayer(new IAdobeLayerComponent[] { timedComponent.Component })
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
            var results = new List<HowToRenderResult>();
            foreach (var renderingDescription in renderingDescriptions)
            {
                if (renderingDescription.What is AxesRenderingDescription axes)
                {
                    results.Add(RenderAxes(axes, maxDuration));
                }
                else if (renderingDescription.What is FunctionRenderingDescription function)
                {
                    results.Add(RenderFunction(function, maxDuration));
                }
                else if (renderingDescription.What is AreaUnderFunctionRenderingDescription auf)
                {
                    results.Add(RenderAreaUnderFunction(auf, maxDuration));
                }
            }

            return results.SelectMany(x => x.Components);

        }

        private HowToRenderResult RenderAxes(AxesRenderingDescription axes,
            AbsoluteTiming maxDuration)
        {
            var axesRenderer = new AxesRenderer(axes, maxDuration)
            {
                EntranceAnimationEnd = new AbsoluteTiming(2)
            };

            return axesRenderer.Render();
        }

        private HowToRenderResult RenderFunction(FunctionRenderingDescription function,
            AbsoluteTiming maxDuration)
        {
            var axesRenderer = new FunctionRenderer(function,
                CreatePointsRenderer(function),
                maxDuration);

            return axesRenderer.Render();
        }

        private HowToRenderResult RenderAreaUnderFunction(AreaUnderFunctionRenderingDescription auf,
            AbsoluteTiming maxDuration)
        {
            var renderer = new AreaUnderFunctionRenderer(auf,
                CreatePointsRenderer(auf.FunctionRenderingDescription),
                maxDuration);

            return renderer.Render();
        }

        private FunctionPointsRenderer CreatePointsRenderer(FunctionRenderingDescription function)
        {
            return new FunctionPointsRenderer(function,
                    function.StartX ?? function.PlotLayoutDescription.AxesLayout.XAxis.MinValue,
                    function.EndX ?? function.PlotLayoutDescription.AxesLayout.XAxis.MaxValue);
        }
    }
}
