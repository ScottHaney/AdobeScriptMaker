using AdobeScriptMaker.UI.Core.DataModels;
using MathRenderingDescriptions.Plot.What;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AdobeScriptMaker.UI.Views.Preview
{
    public class PrimitivesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var items = (IEnumerable<IScriptComponentDataModel>)value;

            var convertedItems = new List<IPreviewCanvasPrimitive>();

            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item is AxesDataModel axesDataModel)
                    {
                        var axes = (AxesRenderingDescription)axesDataModel.ToRenderingData();

                        var topLeft = new Point((int)axes.PlotLayoutDescription.TopLeft.X, (int)axes.PlotLayoutDescription.TopLeft.Y);
                        var yLength = axes.PlotLayoutDescription.AxesLayout.YAxis.Length;
                        var xLength = axes.PlotLayoutDescription.AxesLayout.XAxis.Length;

                        var intersectionPoint = new Point((int)topLeft.X, (int)(topLeft.Y + yLength));
                        convertedItems.Add(new PreviewCanvasLinePrimitive(topLeft, intersectionPoint));
                        convertedItems.Add(new PreviewCanvasLinePrimitive(intersectionPoint, new Point((int)(intersectionPoint.X + xLength), intersectionPoint.Y)));
                    }
                }
            }

            return new PreviewCanvasPrimitives(convertedItems);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
