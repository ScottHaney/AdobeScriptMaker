using AdobeScriptMaker.UI.ViewModels.ScriptBuilder;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.ViewModels.Timeline
{
    public partial class TimelineComponentViewModel : ObservableObject
    {
        public ScriptBuilderComponentViewModel WrappedComponent { get; set; }

        [ObservableProperty]
        private double startPosition;

        [ObservableProperty]
        private double width;
    }
}
