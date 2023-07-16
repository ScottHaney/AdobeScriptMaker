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
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            MainWindow.DataContext = new DesignTimeScriptBuilderViewModel();
        }
    }
}
