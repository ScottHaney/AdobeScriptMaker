using AdobeScriptMaker.UI.Core.DataModels;
using AdobeScriptMaker.UI.Core.ScriptBuilder.Parameters;
using AdobeScriptMaker.UI.Core.Timeline;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdobeScriptMaker.UI.Core.ScriptBuilder
{
    public partial class ScriptBuilderComponentViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private IScriptComponentDataModel componentData;

        [ObservableProperty]
        private IEnumerable<IScriptComponentDataModel> samplePrimitives;

        [RelayCommand]
        private void Selected()
        {
            WeakReferenceMessenger.Default.Send(new AddTimelineComponentMessage(this));
        }
    }
}
