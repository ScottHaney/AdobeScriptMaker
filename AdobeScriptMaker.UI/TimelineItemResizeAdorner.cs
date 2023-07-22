using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace AdobeScriptMaker.UI
{
    public class TimelineItemResizeAdorner : Adorner
    {
        private VisualCollection _visuals;
        private Thumb _resizeLeft;
        private Thumb _resizeRight;

        public TimelineItemResizeAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            _visuals = new VisualCollection(this);
            _resizeLeft = new Thumb() { Background = Brushes.Blue };
            _resizeRight = new Thumb() { Background = Brushes.Blue };

            _visuals.Add(_resizeLeft);
            _visuals.Add(_resizeRight);
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        protected override int VisualChildrenCount => _visuals.Count;

        protected override Size ArrangeOverride(Size finalSize)
        {
            _resizeLeft.Arrange(new Rect(new Point(0, 0), new Point(10, AdornedElement.DesiredSize.Height)));
            _resizeRight.Arrange(new Rect(new Point(AdornedElement.DesiredSize.Width - 10, 0), new Point(AdornedElement.DesiredSize.Width, AdornedElement.DesiredSize.Height)));

            return finalSize;
        }
    }
}
