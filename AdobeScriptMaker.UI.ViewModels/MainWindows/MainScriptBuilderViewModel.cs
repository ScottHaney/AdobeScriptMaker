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
    public partial class MainScriptBuilderViewModel : ObservableObject
    {
        [RelayCommand]
        private void Generate()
        {
            WeakReferenceMessenger.Default.Send(new GenerateScriptMessage());
        }
    }
}
