using AdobeScriptMaker.UI.Core.ScriptBuilder.Parameters;
using AdobeScriptMaker.UI.Core.Timeline;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private List<IScriptBuilderParameter> parameters;

        public TimelineViewModel TimelineReference { get; set; }


        [RelayCommand]
        private void Selected()
        {
            if (TimelineReference != null)
            {
                double start;
                if (!TimelineReference.Components.Any())
                    start = 0;
                else
                    start = TimelineReference.Components.Max(x => x.End);

                TimelineReference.Components.Add(new TimelineComponentViewModel() { WrappedComponent = this, Start = start, End = start + 100 });

            }
        }
    }
}
