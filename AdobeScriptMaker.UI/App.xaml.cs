using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AdobeScriptMaker.UI.Core.ScriptBuilder;
using AdobeScriptMaker.UI.Core.DesignTimeData;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using AdobeScriptMaker.UI.Core.Timeline;
using AdobeScriptMaker.UI.Core.ScriptBuilder.Parameters;
using Prism.Unity;
using Prism.Ioc;
using Prism.Mvvm;
using AdobeScriptMaker.UI.Views.Timeline;
using AdobeScriptMaker.UI.Views.ScriptComponents;
using Prism.Modularity;
using AdobeScriptMaker.UI.PrismModules;
using AdobeScriptMaker.UI.Core.MainWindows;
using AdobeScriptMaker.UI.Views.Preview;
using AdobeScriptMaker.UI.Core.Preview;
using MathRenderingDescriptions.Plot.What;
using MathRenderingDescriptions.Plot;

namespace AdobeScriptMaker.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var w = (MainWindow)Container.Resolve(typeof(MainWindow));
            return w;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<Timeline>(() => new TimelineViewModel() { Width = 1000, Position = -1 });
            ViewModelLocationProvider.Register<ScriptComponents>(() => CreateScriptComponentsViewModel());
            ViewModelLocationProvider.Register<MainWindow>(() => new MainScriptBuilderViewModel());
            ViewModelLocationProvider.Register<Preview>(() => new PreviewViewModel()
            {
                Primitives = new[]
                {
                    new AxesRenderingDescription("test",
                        new PlotLayoutDescription(
                            new PlotAxesLayoutDescription(
                                new PlotAxisLayoutDescription(100, 0, 100), new PlotAxisLayoutDescription(100, 0, 100)),
                                new System.Drawing.PointF(10, 10)))
                }
            });
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule<TimelineModule>();
            moduleCatalog.AddModule<ScriptComponentsModule>();
            moduleCatalog.AddModule<PreviewModule>();
        }

        private ScriptBuilderComponentsViewModel CreateScriptComponentsViewModel()
        {
            var dataContext = new ScriptBuilderComponentsViewModel();

            var component = new ScriptBuilderComponentViewModel() { Name = "Plot Axes" };
            component.Parameters.Add(new ScriptBuilderNumericParameter() { Name = "X Range", DefaultValue = 100, MinValue = 0, MaxValue = double.MaxValue });
            component.Parameters.Add(new ScriptBuilderNumericParameter() { Name = "Y Range", DefaultValue = 100, MinValue = 0, MaxValue = double.MaxValue });

            component.SamplePrimitives = new[]
                {
                    new AxesRenderingDescription("test",
                        new PlotLayoutDescription(
                            new PlotAxesLayoutDescription(
                                new PlotAxisLayoutDescription(100, 0, 100), new PlotAxisLayoutDescription(100, 0, 100)),
                                new System.Drawing.PointF(0, 0)))
                };

            dataContext.Components.Add(component);
            return dataContext;
        }
    }
}
