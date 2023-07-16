using AdobeScriptMaker.UI.ViewModels.Timeline;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.ViewModels.ScriptBuilder
{
    public partial class ScriptBuilderViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<ScriptBuilderComponentViewModel> components;

        [ObservableProperty]
        private TimelineViewModel timeLine;
    }
}
