using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AdobeScriptMaker.UI
{
    public class TimelineTrackPanel : Panel
    {
        public static double GetLeft(DependencyObject obj)
        {
            return (double)obj.GetValue(LeftProperty);
        }

        public static void SetLeft(DependencyObject obj, double value)
        {
            obj.SetValue(LeftProperty, value);
        }

        // Using a DependencyProperty as the backing store for Left.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.RegisterAttached("Left", typeof(double), typeof(TimelineTrackPanel), new PropertyMetadata(0.0));

        public static double GetRight(DependencyObject obj)
        {
            return (double)obj.GetValue(RightProperty);
        }

        public static void SetRight(DependencyObject obj, double value)
        {
            obj.SetValue(RightProperty, value);
        }

        // Using a DependencyProperty as the backing store for Right.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RightProperty =
            DependencyProperty.RegisterAttached("Right", typeof(double), typeof(TimelineTrackPanel), new PropertyMetadata(0.0));

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                var actualVisualChild = VisualTreeHelper.GetChild(child, 0);
                var left = GetLeft(actualVisualChild);
                var right = GetRight(actualVisualChild);

                child.Arrange(new Rect(new Point(left, 0), new Point(right, finalSize.Height)));
            }

            return finalSize;
        }
    }
}
