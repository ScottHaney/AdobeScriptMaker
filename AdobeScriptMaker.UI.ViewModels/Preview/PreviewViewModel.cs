using CommunityToolkit.Mvvm.ComponentModel;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.UI.Core.Preview
{
    public partial class PreviewViewModel : ObservableObject
    {
        [ObservableProperty]
        private IEnumerable<IWhatToRender> primitives;
    }
}
