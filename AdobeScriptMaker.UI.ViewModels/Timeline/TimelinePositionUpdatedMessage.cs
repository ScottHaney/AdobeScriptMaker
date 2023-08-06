using AdobeScriptMaker.UI.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.UI.Core.Timeline
{
    public class TimelinePositionUpdatedMessage
    {
        public readonly double Position;
        public readonly IEnumerable<IScriptComponentDataModel> Components;

        public TimelinePositionUpdatedMessage(double position,
            IEnumerable<IScriptComponentDataModel> components)
        {
            Position = position;
            Components = components;
        }
    }
}
