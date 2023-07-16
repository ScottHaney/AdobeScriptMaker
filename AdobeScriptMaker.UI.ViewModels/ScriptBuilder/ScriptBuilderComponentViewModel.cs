using AdobeScriptMaker.UI.ViewModels.ScriptBuilder.Parameters;
using AdobeScriptMaker.UI.ViewModels.Timeline;
using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class ScriptBuilderComponentViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private List<IScriptBuilderParameter> parameters;

        public TimelineViewModel TimelineReference { get; set; }


        [RelayCommand]
        private void Selected()
        {
            if (TimelineReference != null)
                TimelineReference.Components.Add(new TimelineComponentViewModel() { WrappedComponent = this });
        }
    }
}
