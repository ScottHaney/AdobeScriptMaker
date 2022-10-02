using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class LayoutResultsComposite : ILayoutResults
    {
        public readonly ILayoutResults[] Items;
        public RectangleF BoundingBox => GetBoundingBox();

        public LayoutResultsComposite(params ILayoutResults[] items)
        {
            Items = items ?? Array.Empty<ILayoutResults>();
        }

        public IEnumerable<ILayoutResult> GetResults()
        {
            return Items.SelectMany(x => x.GetResults());
        }

        public void ShiftDown(float diff)
        {
            foreach (var item in Items)
                item.ShiftDown(diff);
        }

        private RectangleF GetBoundingBox()
        {
            var boundingBoxes = Items.Select(x => x.BoundingBox).ToList();

            var left = boundingBoxes.Min(x => x.Left);
            var top = boundingBoxes.Max(x => x.Top);
            var right = boundingBoxes.Max(x => x.Right);
            var bottom = boundingBoxes.Min(x => x.Bottom);

            return new RectangleF(left,
                top,
                right - left,
                bottom - top);
        }
    }
}
