using System;
using System.Collections.Generic;
using System.Text;

namespace DirectRendering.Text
{
    public class SequenceDrawing : PrimitiveDrawing
    {
        public readonly SequenceValue[] Values;
        public readonly string StartText;
        public readonly double StartTime;

        public SequenceDrawing(string startText,
            double startTime,
            params SequenceValue[] values)
        {
            StartText = startText;
            StartTime = startTime;
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
