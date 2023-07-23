using AdobeScriptMaker.UI.Core.Timeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.UI.Core.MainWindows
{
    public class ReceiveTimelineComponentsMessage
    {
        public readonly IEnumerable<TimelineComponentViewModel> Components;
        public readonly double Width;

        public ReceiveTimelineComponentsMessage(IEnumerable<TimelineComponentViewModel> components,
            double width)
        {
            Components = components;
            Width = width;
        }
    }
}
