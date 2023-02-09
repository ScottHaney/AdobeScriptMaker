using RenderingDescriptions.What;
using System;

namespace IllustratorRenderingDescriptions.NavyDigits.What
{
    public class NavyDigitsRenderingDescription : IWhatToRender
    {
        public readonly float Dimension;

        public NavyDigitsRenderingDescription(string uniqueName,
            float dimension)
            : base(uniqueName)
        {
            Dimension = dimension;
        }
    }
}
