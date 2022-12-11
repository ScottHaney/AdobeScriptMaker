using System;
using RenderingDescriptions.What;
using RenderingDescriptions.When;
using RenderingDescriptions.How;

namespace RenderingDescriptions
{
    public class RenderingDescription
    {
        public readonly IWhatToRender What;
        public readonly AbsoluteTiming WhenToStart;
        public readonly IHowToRender How;

        public RenderingDescription(IWhatToRender what,
            AbsoluteTiming whenToStart,
            IHowToRender how)
        {
            What = what;
            WhenToStart = whenToStart;
            How = how;
        }
    }
}
