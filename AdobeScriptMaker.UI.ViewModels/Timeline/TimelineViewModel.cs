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

namespace AdobeScriptMaker.UI.Core.Timeline
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
            {
                var index = components.IndexOf(message.Component);

                double maxValue;
                if (index + 1 < components.Count)
                    maxValue = components[index + 1].Start;
                else
                    maxValue = width;

                var updatedValue = message.Component.End + message.SizeChange;
                if (updatedValue <= maxValue && updatedValue >= message.Component.Start)
                    message.Component.End += message.SizeChange;
            }
            else if (message.Direction == ResizeDirection.Start)
            {
                var index = components.IndexOf(message.Component);

                double minValue;
                if (index - 1 >= 0)
                    minValue = components[index - 1].End;
                else
                    minValue = 0;

                var updatedValue = message.Component.Start + message.SizeChange;
                if (updatedValue >= minValue && updatedValue <= message.Component.End)
                    message.Component.Start += message.SizeChange;
            }
        }
    }
}
