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
        public IEnumerable<TimedAdobeLayerComponent> Components;

        public HowToRenderResult(IEnumerable<TimedAdobeLayerComponent> components)
        {
            Components = components;
        }

        public HowToRenderResult(params TimedAdobeLayerComponent[] components)
        {
            Components = components;
        }
    }
}
