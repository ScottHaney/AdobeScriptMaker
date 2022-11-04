using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering
{
    public class SliderControl : PrimitiveDrawing
    {
        public readonly SliderValue[] Values;

        public string Name { get; set; }

        public SliderControl(params SliderValue[] values)
        {
            Values = values ?? Array.Empty<SliderValue>();
        }
    }

    public class SliderValue
    {
        public readonly double Time;
        public readonly double Value;

        public SliderValue(double time, double value)
        {
            Time = time;
            Value = value;
        }
    }
}
