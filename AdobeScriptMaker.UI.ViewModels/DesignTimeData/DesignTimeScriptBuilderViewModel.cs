using AdobeScriptMaker.UI.ViewModels.ScriptBuilder;
using AdobeScriptMaker.UI.ViewModels.ScriptBuilder.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.ViewModels.DesignTimeData
{
    public class DesignTimeScriptBuilderViewModel
    {
        public List<ScriptBuilderComponentViewModel> Components { get; set; }

        public DesignTimeScriptBuilderViewModel()
        {
            Components = new List<ScriptBuilderComponentViewModel>();

            var component = new ScriptBuilderComponentViewModel();
            component.Name = "Test Component";

            Components.Add(component);
        }
    }
}
