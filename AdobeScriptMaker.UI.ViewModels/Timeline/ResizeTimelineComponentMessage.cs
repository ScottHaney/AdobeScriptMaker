using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.ViewModels.Timeline
{
    public class ResizeTimelineComponentMessage
    {
        public readonly TimelineComponentViewModel Component;
        public readonly double SizeChange;
        public readonly ResizeDirection Direction;

        public ResizeTimelineComponentMessage(TimelineComponentViewModel component,
            double sizeChange,
            ResizeDirection direction)
        {
            Component = component;
            SizeChange = sizeChange;
            Direction = direction;
        }
    }

    public enum ResizeDirection
    {
        Start,
        End
    }
}
