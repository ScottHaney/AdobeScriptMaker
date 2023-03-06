using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.ViewModels.ScriptBuilder.Parameters
{
    public class ScriptBuilderNumericParameter : IScriptBuilderParameter
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double DefaultValue { get; set; }
    }
}
