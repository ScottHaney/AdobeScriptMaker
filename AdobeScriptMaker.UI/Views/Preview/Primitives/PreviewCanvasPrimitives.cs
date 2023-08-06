using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdobeScriptMaker.UI.Views.Preview.Primitives
{
    public class PreviewCanvasPrimitives
    {
        public readonly IEnumerable<IPreviewCanvasPrimitive> Items;

        public PreviewCanvasPrimitives(IEnumerable<IPreviewCanvasPrimitive> items)
        {
            Items = items;
        }
    }
}
