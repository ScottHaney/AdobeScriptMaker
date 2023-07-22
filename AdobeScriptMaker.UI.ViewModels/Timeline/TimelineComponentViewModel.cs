using AdobeScriptMaker.UI.Core.ScriptBuilder;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.Core.Timeline
{
    public partial class TimelineComponentViewModel : ObservableObject
    {
        public ScriptBuilderComponentViewModel WrappedComponent { get; set; }

        [ObservableProperty]
        private double start;

        [ObservableProperty]
        private double end;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg">This has to be of type object otherwise the binding will fail so just cast it to a double as a hack to get it working</param>
        [RelayCommand]
        private void UpdateEnd(object arg)
        {
            WeakReferenceMessenger.Default.Send(new ResizeTimelineComponentMessage(this, (double)arg, ResizeDirection.End));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg">This has to be of type object otherwise the binding will fail so just cast it to a double as a hack to get it working</param>
        [RelayCommand]
        private void UpdateStart(object arg)
        {
            WeakReferenceMessenger.Default.Send(new ResizeTimelineComponentMessage(this, (double)arg, ResizeDirection.Start));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg">This has to be of type object otherwise the binding will fail so just cast it to a double as a hack to get it working</param>
        [RelayCommand]
        private void UpdatePosition(object arg)
        {
            WeakReferenceMessenger.Default.Send(new RepositionTimelineComponentMessage(this, (double)arg));
        }
    }
}
