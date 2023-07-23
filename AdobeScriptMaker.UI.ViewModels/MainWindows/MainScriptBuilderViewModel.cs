using AdobeScriptMaker.Core;
using AdobeScriptMaker.Core.ComponentsConverters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MathRenderingDescriptions.Plot;
using MathRenderingDescriptions.Plot.What;
using RenderingDescriptions;
using RenderingDescriptions.Timing;
using RenderingDescriptions.When;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AdobeScriptMaker.UI.Core.MainWindows
{
    public partial class MainScriptBuilderViewModel : ObservableObject,
        IRecipient<ReceiveTimelineComponentsMessage>
    {
        public MainScriptBuilderViewModel()
        {
            WeakReferenceMessenger.Default.Register<ReceiveTimelineComponentsMessage>(this);
        }

        public void Receive(ReceiveTimelineComponentsMessage message)
        {
            foreach (var component in message.Components)
            {
                if (component.Name == "Plot Axes")
                {
                    var plotLayoutDescription = new PlotLayoutDescription(
                        new PlotAxesLayoutDescription(
                        new PlotAxisLayoutDescription(690, 0, 5),
                        new PlotAxisLayoutDescription(690, 0, 5)), new PointF(100, 300));

                    var axes = new AxesRenderingDescription("Axes",
                        plotLayoutDescription);

                    var axesToRender = new RenderingDescription(axes, new TimingForRender(new AbsoluteTiming(0), new AbsoluteTiming(message.Width)) { EntranceAnimationDuration = new AbsoluteTiming(0.5) }, null);
                    
                    var converter = new UpdatedComponentsConverter();
                    var converted = converter.Convert(new List<RenderingDescription>() { axesToRender });

                    var scriptCreator = new ComponentsScriptCreator();
                    var script = scriptCreator.Visit(converted);
                }
            }
        }

        [RelayCommand]
        private void Generate()
        {
            //Slightly convoluted. This message causes the Receive(ReceiveTimelineComponentsMessage message) call above to get run which contains the actual logic
            WeakReferenceMessenger.Default.Send(new RequestTimelineComponentsMessage());
        }
    }
}
