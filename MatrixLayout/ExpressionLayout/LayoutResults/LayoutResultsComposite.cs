using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class LayoutResultsComposite : ILayoutResults
    {
        private readonly ILayoutResults[] _items;
        public RectangleF BoundingBox => GetBoundingBox();

        public LayoutResultsComposite(params ILayoutResults[] items)
        {
            _items = items ?? Array.Empty<ILayoutResults>();
        }

        public IEnumerable<ILayoutResult> GetResults()
        {
            return _items.SelectMany(x => x.GetResults());
        }

        private RectangleF GetBoundingBox()
        {
            var boundingBoxes = _items.Select(x => x.BoundingBox).ToList();

            var left = boundingBoxes.Min(x => x.Left);
            var top = boundingBoxes.Max(x => x.Top);
            var right = boundingBoxes.Max(x => x.Right);
            var bottom = boundingBoxes.Min(x => x.Bottom);

            return new RectangleF(left,
                top,
                right - left,
                top - bottom);
        }
    }
}
