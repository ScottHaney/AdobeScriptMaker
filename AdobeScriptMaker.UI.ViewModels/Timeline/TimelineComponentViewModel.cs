using AdobeScriptMaker.UI.ViewModels.ScriptBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.ViewModels.Timeline
{
    public class TimelineComponentViewModel
    {
        public ScriptBuilderComponentViewModel WrappedComponent { get; set; }
        public double StartPosition { get; set; }
        public double Width { get; set; }
    }
}
