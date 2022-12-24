using RenderingDescriptions.When;
using System;
using System.Collections.Generic;
using System.Text;

namespace RenderingDescriptions.Timing
{
    public interface ITimingForRender
    {
        AbsoluteTiming WhenToStart { get; }
        AbsoluteTiming RenderDuration { get; }
        AbsoluteTiming EntranceAnimationDuration { get; }
        AbsoluteTiming ExitAnimationDuration { get; }
    }

    public class TimingForRender : ITimingForRender
    {
        public AbsoluteTiming WhenToStart { get; }

        public AbsoluteTiming RenderDuration { get; }

        public AbsoluteTiming EntranceAnimationDuration { get; set; } = new AbsoluteTiming(0);

        public AbsoluteTiming ExitAnimationDuration { get; set; } = new AbsoluteTiming(0);

        public TimingForRender(AbsoluteTiming whenToStart,
            AbsoluteTiming renderDuration)
        {
            WhenToStart = whenToStart;
            RenderDuration = renderDuration;
        }
    }
}
