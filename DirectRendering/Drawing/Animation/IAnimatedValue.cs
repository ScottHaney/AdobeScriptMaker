using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering.Drawing.Animation
{
    public interface IAnimatedValue<T>
    {
        bool IsAnimated { get; }
        IEnumerable<ValueAtTime<T>> GetValues();
    }

    public class ValueAtTime<T>
    {
        public readonly T Value;
        public readonly AnimationTime Time;

        public ValueAtTime(T value, AnimationTime time)
        {
            Value = value;
            Time = time;
        }

        public ValueAtTime(T value)
            : this(value, AnimationTime.AllTimes)
        { }
    }

    public class AnimationTime
    {
        private readonly double? _internalTime;

        public static readonly AnimationTime AllTimes = new AnimationTime();

        public double? Time => _internalTime;

        public AnimationTime(double value)
        {
            _internalTime = value;
        }

        private AnimationTime()
        { }
    }
}
