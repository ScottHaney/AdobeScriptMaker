using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.ViewModels.Timeline
{
    public partial class TimelineViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<TimelineComponentViewModel> components = new ObservableCollection<TimelineComponentViewModel>();
    }
}
