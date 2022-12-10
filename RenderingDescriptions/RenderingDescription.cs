using System;
using RenderingDescriptions.What;
using RenderingDescriptions.When;
using RenderingDescriptions.How;

namespace RenderingDescriptions
{
    public class RenderingDescription
    {
        public readonly IWhatToRender What;
        public readonly IPointInTime WhenToStart;
        public readonly IHowToRender How;

        public RenderingDescription(IWhatToRender what,
            IPointInTime whenToStart,
            IHowToRender how)
        {
            What = what;
            WhenToStart = whenToStart;
            How = how;
        }
    }
}
