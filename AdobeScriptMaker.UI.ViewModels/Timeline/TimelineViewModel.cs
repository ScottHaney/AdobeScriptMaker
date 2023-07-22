using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdobeScriptMaker.UI.ViewModels.Timeline
{
    public partial class TimelineViewModel : ObservableRecipient, IRecipient<ResizeTimelineComponentMessage>
    {
        [ObservableProperty]
        private ObservableCollection<TimelineComponentViewModel> components = new ObservableCollection<TimelineComponentViewModel>();

        [ObservableProperty]
        private double width;

        public TimelineViewModel()
        {
            WeakReferenceMessenger.Default.Register<ResizeTimelineComponentMessage>(this);
        }

        public void Receive(ResizeTimelineComponentMessage message)
        {
            if (message.Direction == ResizeDirection.End)
                message.Component.End += message.SizeChange;
            else if (message.Direction == ResizeDirection.Start)
                message.Component.Start += message.SizeChange;
        }
    }
}
