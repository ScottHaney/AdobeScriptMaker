using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.Core.ScriptBuilder.Parameters
{
    public class ScriptBuilderNumericParameter : IScriptBuilderParameter
    {
        public string Name { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double Value { get; set; }
    }
}
