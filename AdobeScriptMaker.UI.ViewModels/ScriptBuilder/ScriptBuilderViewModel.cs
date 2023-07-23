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
    public partial class ScriptBuilderViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<ScriptBuilderComponentViewModel> components = new List<ScriptBuilderComponentViewModel>();

        [ObservableProperty]
        private TimelineViewModel timeLine;

        [RelayCommand]
        private void Generate()
        {
            foreach (var component in components)
            {
                if (component.Name == "Plot Axes")
                {
                    var plotLayoutDescription = new PlotLayoutDescription(
                        new PlotAxesLayoutDescription(
                        new PlotAxisLayoutDescription(690, 0, 5),
                        new PlotAxisLayoutDescription(690, 0, 5)), new PointF(100, 300));

                    var axes = new AxesRenderingDescription("Axes",
                        plotLayoutDescription);

                    var axesToRender = new RenderingDescription(axes, new TimingForRender(new AbsoluteTiming(0), new AbsoluteTiming(TimeLine.Width)) { EntranceAnimationDuration = new AbsoluteTiming(0.5) }, null);
                    
                    var converter = new UpdatedComponentsConverter();
                    var converted = converter.Convert(new List<RenderingDescription>() { axesToRender });

                    var scriptCreator = new ComponentsScriptCreator();
                    var script = scriptCreator.Visit(converted);
                }
            }
        }
    }
}
