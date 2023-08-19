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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace WPFCustomControls
{
    public class SideSheet : ContentControl
    {
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(SideSheet), new PropertyMetadata(false, IsActiveChanged));

        static SideSheet()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SideSheet), new FrameworkPropertyMetadata(typeof(SideSheet)));
        }

        public SideSheet()
        {
            Loaded += SideSheet_Loaded;
        }

        private void SideSheet_Loaded(object sender, RoutedEventArgs e)
        {
            RenderTransform = new TranslateTransform(ActualWidth, 0);
        }

        private static void IsActiveChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var sideSheet = (SideSheet)o;
            var isActive = (bool)args.NewValue;

            sideSheet.RenderTransform = new TranslateTransform(isActive ? 0 : sideSheet.ActualWidth, 0);

            /*sideSheet.SetValue(WidthProperty, DependencyProperty.UnsetValue);

            var targetValue = isActive ? sideSheet.DesiredSize.Width : 0;

            var animation = new DoubleAnimation() { To = targetValue, Duration = new Duration(TimeSpan.FromSeconds(2)) };
            sideSheet.BeginAnimation(WidthProperty, animation);*/
        }
    }
}
