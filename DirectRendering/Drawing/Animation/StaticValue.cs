using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering.Drawing.Animation
{
    public class StaticValue<T> : IAnimatedValue<T>
    {
        private readonly T _value;

        public bool IsAnimated => false;

        public StaticValue(T value)
        {
            _value = value;
        }

        public IEnumerable<ValueAtTime<T>> GetValues()
        {
            yield return new ValueAtTime<T>(_value);
        }
    }
}
