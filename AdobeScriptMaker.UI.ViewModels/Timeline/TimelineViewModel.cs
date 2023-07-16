using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.ViewModels.Timeline
{
    public class TimelineViewModel
    {
        public ObservableCollection<TimelineComponentViewModel> Components { get; set; } = new ObservableCollection<TimelineComponentViewModel>();
    }
}
