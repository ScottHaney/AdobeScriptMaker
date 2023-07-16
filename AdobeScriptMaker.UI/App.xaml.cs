using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AdobeScriptMaker.UI.ViewModels.ScriptBuilder;
using AdobeScriptMaker.UI.ViewModels.DesignTimeData;

namespace AdobeScriptMaker.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new DesignTimeScriptBuilderViewModel();
            MainWindow = mainWindow;
            mainWindow.Show();
        }
    }
}
