﻿using System;
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
using AdobeScriptMaker.UI.Views.PropertiesEditor;
using AdobeScriptMaker.UI.Core.DataModels;
using AdobeScriptMaker.UI.Views.Preview.Primitives;

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
            containerRegistry.RegisterSingleton<IPrimitivesConverter, PrimitivesConverter>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            var timeLineViewModel = new TimelineViewModel();

            ViewModelLocationProvider.Register<Timeline>(() => timeLineViewModel);
            ViewModelLocationProvider.Register<ScriptComponents>(() => CreateScriptComponentsViewModel());
            ViewModelLocationProvider.Register<MainWindow>(() => new MainScriptBuilderViewModel());
            ViewModelLocationProvider.Register<Preview>(() => new PreviewViewModel());
            ViewModelLocationProvider.Register<PropertiesEditor>(() => timeLineViewModel);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);

            moduleCatalog.AddModule<TimelineModule>();
            moduleCatalog.AddModule<ScriptComponentsModule>();
            moduleCatalog.AddModule<PreviewModule>();
            moduleCatalog.AddModule<PropertiesEditorModule>();
        }

        private ScriptBuilderComponentsViewModel CreateScriptComponentsViewModel()
        {
            var dataContext = new ScriptBuilderComponentsViewModel();

            var displayAxes = new AxesDataModel();
            displayAxes.SetParameter("X Top", 100);
            displayAxes.SetParameter("Y Top", 150);

            var component = new ScriptBuilderComponentViewModel() { Name = "Plot Axes", ComponentData = displayAxes };

            component.SamplePrimitives = new[]
                {
                    new AxesDataModel()
                };

            dataContext.Components.Add(component);
            return dataContext;
        }
    }
}
