using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot
{
    public class AxesRenderingDescription : IWhatToRender
    {
        public readonly PlotLayoutDescription PlotLayoutDescription;

        public AxesRenderingDescription(PlotLayoutDescription plotLayoutDescription)
        {
            PlotLayoutDescription = plotLayoutDescription;
        }
    }
}
