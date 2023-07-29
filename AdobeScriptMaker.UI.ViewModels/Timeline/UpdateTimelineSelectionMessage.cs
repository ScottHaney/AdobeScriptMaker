using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.UI.Core.Timeline
{
    public class UpdateTimelineSelectionMessage
    {
        public readonly TimelineComponentViewModel SelectedItem;

        public UpdateTimelineSelectionMessage(TimelineComponentViewModel selectedItem)
        {
            SelectedItem = selectedItem;
        }
    }
}
