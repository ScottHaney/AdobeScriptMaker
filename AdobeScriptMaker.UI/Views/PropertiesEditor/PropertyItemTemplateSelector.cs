using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AdobeScriptMaker.UI.Views.PropertiesEditor
{
    public class PropertyItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DoubleTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
            DependencyObject container)
        {
            return DoubleTemplate;
        }
    }
}
