using AdobeComponents.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace RenderingDescriptions.How
{
    public interface IHowToRender
    {
        HowToRenderResult Render();
    }

    public class HowToRenderResult
    {
        public IEnumerable<IAdobeLayerComponent> Components;
        public double Duration;

        public HowToRenderResult(IEnumerable<IAdobeLayerComponent> components,
            double duration)
        {
            Components = components;
            Duration = duration;
        }

    }
}
