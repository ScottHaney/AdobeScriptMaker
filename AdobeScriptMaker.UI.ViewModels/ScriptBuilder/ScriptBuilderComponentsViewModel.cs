using AdobeScriptMaker.Core;
using AdobeScriptMaker.Core.ComponentsConverters;
using AdobeScriptMaker.UI.Core.Timeline;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MathRenderingDescriptions.Plot;
using MathRenderingDescriptions.Plot.What;
using RenderingDescriptions;
using RenderingDescriptions.Timing;
using RenderingDescriptions.When;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.Core.ScriptBuilder
{
    public partial class ScriptBuilderComponentsViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<ScriptBuilderComponentViewModel> components = new List<ScriptBuilderComponentViewModel>();
    }
}
