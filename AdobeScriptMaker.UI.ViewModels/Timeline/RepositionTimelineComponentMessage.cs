using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.Core.Timeline
{
    public class RepositionTimelineComponentMessage
    {
        public readonly TimelineComponentViewModel Component;
        public readonly double PositionChange;

        public RepositionTimelineComponentMessage(TimelineComponentViewModel component,
            double positionChange)
        {
            Component = component;
            PositionChange = positionChange;
        }
    }
}
