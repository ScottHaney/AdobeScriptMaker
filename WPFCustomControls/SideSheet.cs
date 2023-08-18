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

        private static void IsActiveChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var uiElement = (UIElement)o;
            var newValue = (bool)args.NewValue;

            var targetValue = newValue ? uiElement.DesiredSize.Width : 0;

            var animation = new DoubleAnimation() { To = targetValue, Duration = new Duration(TimeSpan.FromSeconds(2)) };
            uiElement.BeginAnimation(WidthProperty, animation);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }
    }
}
