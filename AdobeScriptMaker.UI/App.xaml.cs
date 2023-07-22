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

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new DesignTimeScriptBuilderViewModel();
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
