using System;
using System.Collections.Generic;
using System.Text;

namespace RenderingDescriptions.When
{
    public class RelativeTiming : IPointInTime
    {
        public IPointInTime RelativeTo;
        
        public double Delay { get; set; }

        public RelativeTiming(IPointInTime relativeTo)
        {
            RelativeTo = relativeTo;
        }
    }
}
