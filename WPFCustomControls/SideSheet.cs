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
using System.Windows.Controls.Primitives;

namespace WPFCustomControls
{
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(ButtonBase))]
    public class SideSheet : ContentControl
    {
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(SideSheet), new PropertyMetadata(false));

        public static readonly RoutedEvent CloseClickEvent = EventManager.RegisterRoutedEvent(
            name: "CloseClick",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(SideSheet));

        public event RoutedEventHandler CloseClick
        {
            add { AddHandler(CloseClickEvent, value); }
            remove { RemoveHandler(CloseClickEvent, value); }
        }

        static SideSheet()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SideSheet), new FrameworkPropertyMetadata(typeof(SideSheet)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var closeButton = Template.FindName("PART_CloseButton", this) as ButtonBase;
            if (closeButton != null)
                closeButton.Click += CloseButton_Click;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var args = new RoutedEventArgs(CloseClickEvent);
            RaiseEvent(args);
        }
    }
}
