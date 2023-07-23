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
            TimeLine = new TimelineViewModel() { Width = 1000 };

            Components = new List<ScriptBuilderComponentViewModel>();

            var component = new ScriptBuilderComponentViewModel();
            component.Name = "Plot Axes";
            component.TimelineReference = TimeLine;

            component.Parameters.Add(new ScriptBuilderNumericParameter() { Name = "X Range", DefaultValue = 100, MinValue = 0, MaxValue = double.MaxValue });
            component.Parameters.Add(new ScriptBuilderNumericParameter() { Name = "Y Range", DefaultValue = 100, MinValue = 0, MaxValue = double.MaxValue });

            Components.Add(component);
        }
    }
}
