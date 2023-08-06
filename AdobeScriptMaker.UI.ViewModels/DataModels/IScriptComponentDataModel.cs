using AdobeScriptMaker.UI.Core.ScriptBuilder.Parameters;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.UI.Core.DataModels
{
    public interface IScriptComponentDataModel
    {
        IEnumerable<IScriptBuilderParameter> Parameters { get; }
        IWhatToRender ToRenderingData();
    }
}
