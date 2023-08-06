using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace AdobeScriptMaker.UI.Views.Preview.Primitives
{
    public interface IPreviewCanvasPrimitive
    {
        void Draw(DrawingContext drawingContext);
        Rect GetBounds();
    }
}
