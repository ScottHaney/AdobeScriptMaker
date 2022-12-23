using System;
using RenderingDescriptions.What;
using RenderingDescriptions.When;
using RenderingDescriptions.How;
using RenderingDescriptions.Timing;

namespace RenderingDescriptions
{
    public class RenderingDescription
    {
        public readonly IWhatToRender What;
        public readonly ITimingForRender Timing;
        public readonly IHowToRender How;

        public RenderingDescription(IWhatToRender what,
            ITimingForRender timing,
            IHowToRender how)
        {
            What = what;
            Timing = timing;
            How = how;
        }
    }
}
