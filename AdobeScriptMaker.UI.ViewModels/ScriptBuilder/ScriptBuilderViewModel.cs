using AdobeScriptMaker.UI.ViewModels.Timeline;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.ViewModels.ScriptBuilder
{
    public class ScriptBuilderViewModel
    {
        public List<ScriptBuilderComponentViewModel> Components { get; set; }
        public TimelineViewModel TimeLine { get; set; }
    }
}
