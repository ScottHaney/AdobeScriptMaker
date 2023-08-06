using AdobeScriptMaker.UI.Core.DataModels;
using AdobeScriptMaker.UI.Core.ScriptBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.UI.Core.DesignTimeData
{
    public class ScriptComponentsViewModel
    {
        public List<ScriptComponentViewModel> Components { get; set; } = new List<ScriptComponentViewModel>();

        public string Name { get; set; }

        public IScriptComponentDataModel ComponentData { get; set; }

        public IEnumerable<IScriptComponentDataModel> SamplePrimitives { get; set; }

        public ScriptComponentsViewModel()
        {
            Components.Add(new ScriptComponentViewModel()
            {
                Name = "Plot Axes",
                ComponentData = new AxesDataModel(),
                SamplePrimitives = new[]
                {
                    new AxesDataModel()
                }
            });
        }
    }

    public class ScriptComponentViewModel
    {
        public string Name { get; set; }

        public IScriptComponentDataModel ComponentData { get; set; }

        public IEnumerable<IScriptComponentDataModel> SamplePrimitives { get; set; }
    }
}
