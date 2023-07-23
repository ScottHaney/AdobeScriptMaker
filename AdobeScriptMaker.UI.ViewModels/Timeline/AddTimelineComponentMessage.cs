using AdobeScriptMaker.UI.Core.ScriptBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.Core.Timeline
{
    public class AddTimelineComponentMessage
    {
        public readonly ScriptBuilderComponentViewModel Component;

        public AddTimelineComponentMessage(ScriptBuilderComponentViewModel component)
        {
            Component = component;
        }
    }
}
