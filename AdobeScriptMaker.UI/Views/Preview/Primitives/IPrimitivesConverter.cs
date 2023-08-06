using AdobeScriptMaker.UI.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.Views.Preview.Primitives
{
    public interface IPrimitivesConverter
    {
        PreviewCanvasPrimitives Convert(IEnumerable<IScriptComponentDataModel> items);
    }
}
