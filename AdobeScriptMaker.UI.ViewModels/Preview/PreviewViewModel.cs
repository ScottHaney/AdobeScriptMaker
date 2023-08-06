using AdobeScriptMaker.UI.Core.Timeline;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MathRenderingDescriptions.Plot.What;
using MathRenderingDescriptions.Plot;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;
using AdobeScriptMaker.UI.Core.DataModels;

namespace AdobeScriptMaker.UI.Core.Preview
{
    public partial class PreviewViewModel : ObservableObject,
        IRecipient<TimelinePositionUpdatedMessage>
    {
        [ObservableProperty]
        private IEnumerable<IScriptComponentDataModel> primitives;

        public PreviewViewModel()
        {
            WeakReferenceMessenger.Default.Register<TimelinePositionUpdatedMessage>(this);
        }

        public void Receive(TimelinePositionUpdatedMessage message)
        {
            Primitives = message.Components.ToList();
        }
    }
}
