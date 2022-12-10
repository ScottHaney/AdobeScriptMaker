using System;
using System.Collections.Generic;
using System.Text;

namespace RenderingDescriptions.When
{
    public class AbsoluteTiming : IPointInTime
    {
        public readonly double Time;

        public AbsoluteTiming(double time)
        {
            Time = time;
        }
    }
}
