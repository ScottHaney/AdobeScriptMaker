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
    }

    public class TimingForRender : ITimingForRender
    {
        public AbsoluteTiming WhenToStart { get; }

        public AbsoluteTiming RenderDuration { get; }

        public TimingForRender(AbsoluteTiming whenToStart,
            AbsoluteTiming renderDuration)
        {
            WhenToStart = whenToStart;
            RenderDuration = renderDuration;
        }
    }
}
