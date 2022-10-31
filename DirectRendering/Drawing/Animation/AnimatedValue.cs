using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering.Drawing.Animation
{
    public class AnimatedValue<T> : IAnimatedValue<T>
    {
        private readonly ValueAtTime<T>[] _values;

        public AnimatedValue(params ValueAtTime<T>[] values)
        {
            _values = values ?? Array.Empty<ValueAtTime<T>>();
        }

        public IEnumerable<ValueAtTime<T>> GetValues()
        {
            return _values;
        }
    }
}
