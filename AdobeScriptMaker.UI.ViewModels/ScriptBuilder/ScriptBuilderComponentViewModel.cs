using AdobeScriptMaker.UI.ViewModels.ScriptBuilder.Parameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.ViewModels.ScriptBuilder
{
    public class ScriptBuilderComponentViewModel
    {
        public string Name { get; set; }
        public List<IScriptBuilderParameter> Parameters { get; set; }
    }
}
