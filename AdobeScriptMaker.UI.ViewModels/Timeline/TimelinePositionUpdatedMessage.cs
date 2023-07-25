using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.UI.Core.Timeline
{
    public class TimelinePositionUpdatedMessage
    {
        public readonly double Position;
        public readonly IEnumerable<TimelineComponentViewModel> Components;

        public TimelinePositionUpdatedMessage(double position,
            IEnumerable<TimelineComponentViewModel> components)
        {
            Position = position;
            Components = components;
        }
    }
}
