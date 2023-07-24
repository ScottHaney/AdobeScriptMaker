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

            ViewModelLocationProvider.Register<Timeline>(() => new TimelineViewModel() { Width = 1000 });
            ViewModelLocationProvider.Register<ScriptComponents>(() => CreateScriptComponentsViewModel());
            ViewModelLocationProvider.Register<MainWindow>(() => new MainScriptBuilderViewModel());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule<TimelineModule>();
            moduleCatalog.AddModule<ScriptComponentsModule>();
        }

        private ScriptBuilderComponentsViewModel CreateScriptComponentsViewModel()
        {
            var dataContext = new ScriptBuilderComponentsViewModel();

            var component = new ScriptBuilderComponentViewModel() { Name = "Plot Axes" };
            component.Parameters.Add(new ScriptBuilderNumericParameter() { Name = "X Range", DefaultValue = 100, MinValue = 0, MaxValue = double.MaxValue });
            component.Parameters.Add(new ScriptBuilderNumericParameter() { Name = "Y Range", DefaultValue = 100, MinValue = 0, MaxValue = double.MaxValue });

            dataContext.Components.Add(component);
            return dataContext;
        }
    }
}
