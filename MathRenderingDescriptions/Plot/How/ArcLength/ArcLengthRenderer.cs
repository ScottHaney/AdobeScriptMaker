using MathRenderingDescriptions.Plot.What.ArcLength;
using RenderingDescriptions.How;
using RenderingDescriptions.Timing;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.How.ArcLength
{
    public class ArcLengthRenderer : IHowToRender
    {
        private readonly ArcLengthRenderingDescription _description;

        public ArcLengthRenderer(ArcLengthRenderingDescription description)
        {
            _description = description;
        }

        public RenderedComponents Render(ITimingForRender timing)
        {
            throw new NotImplementedException();
        }
    }
}
