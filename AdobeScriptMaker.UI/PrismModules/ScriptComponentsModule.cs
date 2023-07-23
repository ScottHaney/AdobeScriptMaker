﻿using AdobeScriptMaker.UI.Core.ScriptBuilder;
using AdobeScriptMaker.UI.Core.Timeline;
using AdobeScriptMaker.UI.Views.ScriptComponents;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.PrismModules
{
    public class ScriptComponentsModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // use the containerProvider to retrieve the instance of the Prism RegionManager
            // and register the view in this module with a specific region in the app
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.ScriptComponentsRegionName, typeof(ScriptComponents));
        }
    }
}
