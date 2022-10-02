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
                throw new NotImplementedException();
        }

        public IEnumerable<ILayoutResult> GetResults()
        {
            return _results;
        }

        public void ShiftDown(float diff)
        {
            foreach (var result in _results)
                result.ShiftDown(diff);
        }
    }
}
