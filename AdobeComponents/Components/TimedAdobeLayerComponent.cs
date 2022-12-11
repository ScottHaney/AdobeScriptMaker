using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Components
{
    public class TimedAdobeLayerComponent
    {
        public readonly IAdobeLayerComponent Component;
        public readonly double StartTime;
        public readonly double EndTime;

        public TimedAdobeLayerComponent(IAdobeLayerComponent component,
            double startTime,
            double endTime)
        {
            Component = component;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
