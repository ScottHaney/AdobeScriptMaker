using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdobeScriptMaker.UI
{
    public class ResizeableControl : Control
    {
        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XPositionProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("LeftProperty", typeof(double), typeof(ResizeableControl), new PropertyMetadata(0));

        public double Right
        {
            get { return (double)GetValue(RightProperty); }
            set { SetValue(RightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Right.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightProperty =
            DependencyProperty.Register("Right", typeof(double), typeof(ResizeableControl), new PropertyMetadata(0));

        static ResizeableControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeableControl), new FrameworkPropertyMetadata(typeof(ResizeableControl)));
        }
    }
}
