using AdobeScriptMaker.UI.Core.DataModels;
using AdobeScriptMaker.UI.Views.Preview.Primitives;
using MathRenderingDescriptions.Plot.What;
using Prism.Ioc;
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
    public class WPFPrimitivesConverter : IValueConverter
    {
        private readonly IPrimitivesConverter _primitivesConverter;

        public WPFPrimitivesConverter(IPrimitivesConverter primitivesConverter)
        {
            _primitivesConverter = primitivesConverter;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _primitivesConverter.Convert((IEnumerable<IScriptComponentDataModel>)value); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
