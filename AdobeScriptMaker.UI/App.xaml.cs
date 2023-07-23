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

namespace AdobeScriptMaker.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Ioc.Default.ConfigureServices(GetServices().BuildServiceProvider());

            var dataContext = new ScriptBuilderViewModel();

            var component = new ScriptBuilderComponentViewModel() { Name = "Plot Axes" };
            component.Parameters.Add(new ScriptBuilderNumericParameter() { Name = "X Range", DefaultValue = 100, MinValue = 0, MaxValue = double.MaxValue });
            component.Parameters.Add(new ScriptBuilderNumericParameter() { Name = "Y Range", DefaultValue = 100, MinValue = 0, MaxValue = double.MaxValue });

            dataContext.Components.Add(component);

            dataContext.TimeLine = new TimelineViewModel() { Width = 1000 };

            var mainWindow = new MainWindow();
            mainWindow.DataContext = dataContext;
            MainWindow = mainWindow;
            mainWindow.Show();
        }

        private ServiceCollection GetServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<ScriptBuilderComponentViewModel>();
            services.AddTransient<ScriptBuilderViewModel>();
            services.AddTransient<TimelineComponentViewModel>();
            services.AddTransient<TimelineViewModel>();

            return services;
        }
    }
}
