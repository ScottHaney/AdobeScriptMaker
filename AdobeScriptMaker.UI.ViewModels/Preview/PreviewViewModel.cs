using AdobeScriptMaker.UI.Core.Timeline;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MathRenderingDescriptions.Plot.What;
using MathRenderingDescriptions.Plot;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.UI.Core.Preview
{
    public partial class PreviewViewModel : ObservableObject,
        IRecipient<TimelinePositionUpdatedMessage>
    {
        [ObservableProperty]
        private IEnumerable<IWhatToRender> primitives;

        public PreviewViewModel()
        {
            WeakReferenceMessenger.Default.Register<TimelinePositionUpdatedMessage>(this);
        }

        public void Receive(TimelinePositionUpdatedMessage message)
        {
            var updatedPrimitives = new List<IWhatToRender>();
            if (message.Components.Any())
            {
                foreach (var component in message.Components)
                {
                    updatedPrimitives.Add(new AxesRenderingDescription("test",
                        new PlotLayoutDescription(
                            new PlotAxesLayoutDescription(
                                new PlotAxisLayoutDescription(100, 0, 100), new PlotAxisLayoutDescription(100, 0, 100)),
                                new System.Drawing.PointF(0, 0))));
                }
            }

            Primitives = updatedPrimitives;
        }
    }
}
