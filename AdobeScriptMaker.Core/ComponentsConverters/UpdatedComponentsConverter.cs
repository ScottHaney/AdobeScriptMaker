using RenderingDescriptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RenderingDescriptions.When;
using MathRenderingDescriptions.Plot;
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

            var layer = new AdobeLayer(CreateComponents(renderingDescriptions, maxDuration)
                    .ToArray())
            {
                InPoint = 0,
                OutPoint = maxDuration.Time
            };

            layers.Add(layer);

            var defaultComp = new AdobeComposition(layers.ToArray());
            return new AdobeScript(defaultComp);
        }

        public IEnumerable<IAdobeLayerComponent> CreateComponents(List<RenderingDescription> renderingDescriptions,
            AbsoluteTiming maxDuration)
        {
            var results = new List<HowToRenderResult>();
            foreach (var renderingDescription in renderingDescriptions)
            {
                if (renderingDescription.What is AxesRenderingDescription axes)
                {
                    results.Add(RenderAxes(axes, maxDuration));
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
    }
}
