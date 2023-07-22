using AdobeScriptMaker.UI.Core.ScriptBuilder;
using AdobeScriptMaker.UI.Core.ScriptBuilder.Parameters;
using AdobeScriptMaker.UI.Core.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.Core.DesignTimeData
{
    public class DesignTimeScriptBuilderViewModel
    {
        public List<ScriptBuilderComponentViewModel> Components { get; set; }
        public TimelineViewModel TimeLine { get; set; }

        public DesignTimeScriptBuilderViewModel()
        {
            TimeLine = new TimelineViewModel();

            Components = new List<ScriptBuilderComponentViewModel>();

            var component = new ScriptBuilderComponentViewModel();
            component.Name = "Test Component";
            component.TimelineReference = TimeLine;

            Components.Add(component);
        }
    }
}
