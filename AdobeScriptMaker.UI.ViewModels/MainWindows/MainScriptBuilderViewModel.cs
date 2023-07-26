using AdobeScriptMaker.Core;
using AdobeScriptMaker.Core.ComponentsConverters;
using AdobeScriptMaker.UI.Core.Timeline;
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
        IRecipient<TimelinePositionUpdatedMessage>
    {
        [ObservableProperty]
        private double position;

        public MainScriptBuilderViewModel()
        {
            WeakReferenceMessenger.Default.Register<TimelinePositionUpdatedMessage>(this);
        }

        public void Receive(TimelinePositionUpdatedMessage message)
        {
            Position = message.Position;
        }

        [RelayCommand]
        private void Generate()
        {
            WeakReferenceMessenger.Default.Send(new GenerateScriptMessage());
        }

        [RelayCommand]
        private void Loaded()
        {
            WeakReferenceMessenger.Default.Send(new InitializeStateMessage(1000, 0));
        }
    }
}
