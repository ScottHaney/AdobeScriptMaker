using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering.Text
{
    public class SequenceDrawing : PrimitiveDrawing
    {
        public readonly SequenceValue[] Values;

        public SequenceDrawing(params SequenceValue[] values)
        {
            Values = values;
        }
    }

    public class SequenceValue
    {
        public readonly double Value;
        public readonly double Time;

        public SequenceValue(double value,
            double time)
        {
            Value = value;
            Time = time;
        }
    }
}
