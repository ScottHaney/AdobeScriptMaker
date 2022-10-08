using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class LayoutResultsCollection : ILayoutResults
    {
        private readonly ILayoutResult[] _results;
        public RectangleF BoundingBox => GetBoundingBox();

        public LayoutResultsCollection(params ILayoutResult[] results)
        {
            _results = results ?? Array.Empty<ILayoutResult>();
        }

        private RectangleF GetBoundingBox()
        {
            if (_results.Length == 1)
                return _results[0].Bounds;
            else
            {
                var boxes = _results.Select(x => x.Bounds).ToList();
                return new RectangleF(
                    boxes.Min(x => x.Left),
                    boxes.Min(x => x.Top),
                    boxes.Max(x => x.Right) - boxes.Min(x => x.Left),
                    boxes.Max(x => x.Bottom) - boxes.Min(x => x.Top));
            }
        }

        public IEnumerable<ILayoutResult> GetResults()
        {
            return _results;
        }

        public IEnumerable<ILayoutResults> GetComponents()
        {
            yield return this;
        }

        public void ShiftDown(float diff)
        {
            foreach (var result in _results)
                result.ShiftDown(diff);
        }
    }
}
