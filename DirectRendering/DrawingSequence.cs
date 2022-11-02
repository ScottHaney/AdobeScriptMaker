using DirectRendering.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering
{
    public class DrawingSequence
    {
        public readonly TimingContext[] Contexts;

        public DrawingSequence(params IDrawing[] drawings)
        {
            Contexts = new TimingContext[]
            {
                new TimingContext(new AbsoluteTimingContextTime(0), new AbsoluteTimingContextTime(30), drawings)
            };
        }

        public DrawingSequence(params TimingContext[] timingContexts)
        {
            Contexts = timingContexts;
        }
    }

    public class TimingContext
    {
        public readonly TimingContextTime StartTime;
        public readonly TimingContextTime Duration;
        public readonly IDrawing[] Drawings;

        public TimingContext(TimingContextTime startTime,
            TimingContextTime duration,
            params IDrawing[] drawings)
        {
            StartTime = startTime;
            Duration = duration;
            Drawings = drawings;
        }
    }

    public abstract class TimingContextTime
    {
        public abstract double GetAbsoluteTime(double currentTime);
    }

    public class RelativeTimingContextTime : TimingContextTime
    {
        private readonly double _timeOffset;

        public RelativeTimingContextTime(double timeOffset)
        {
            _timeOffset = timeOffset;
        }

        public override double GetAbsoluteTime(double currentTime)
        {
            return currentTime + _timeOffset;
        }
    }

    public class AbsoluteTimingContextTime : TimingContextTime
    {
        private readonly double _time;

        public AbsoluteTimingContextTime(double time)
        {
            _time = time;
        }

        public override double GetAbsoluteTime(double currentTime)
        {
            return _time;
        }
    }
}
