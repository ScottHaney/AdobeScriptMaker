using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What
{
    public class AxesRenderingDescription : IWhatToRender
    {
        public readonly PlotLayoutDescription PlotLayoutDescription;

        public AxesRenderingDescription(string uniqueName,
            PlotLayoutDescription plotLayoutDescription)
            : base(uniqueName)
        {
            PlotLayoutDescription = plotLayoutDescription;
        }
    }
}
