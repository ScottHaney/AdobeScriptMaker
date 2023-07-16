using AdobeScriptMaker.UI.ViewModels.ScriptBuilder.Parameters;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdobeScriptMaker.UI.ViewModels.ScriptBuilder
{
    public partial class ScriptBuilderComponentViewModel
    {
        public string Name { get; set; }
        public List<IScriptBuilderParameter> Parameters { get; set; }

        [RelayCommand]
        private void Selected()
        {

        }
    }


}
