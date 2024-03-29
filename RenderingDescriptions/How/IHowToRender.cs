﻿using AdobeComponents.Components;
using RenderingDescriptions.Timing;
using RenderingDescriptions.When;
using System;
using System.Collections.Generic;
using System.Text;

namespace RenderingDescriptions.How
{
    public interface IHowToRender
    {
        RenderedComponents Render(ITimingForRender timing);
    }

    public class RenderedComponents
    {
        public IEnumerable<TimedAdobeLayerComponent> Components;

        public RenderedComponents(IEnumerable<TimedAdobeLayerComponent> components)
        {
            Components = components;
        }

        public RenderedComponents(params TimedAdobeLayerComponent[] components)
        {
            Components = components;
        }
    }
}
