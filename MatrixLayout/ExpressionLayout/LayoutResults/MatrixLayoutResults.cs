using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class MatrixLayoutResults : ILayoutResults
    {
        public RectangleF BoundingBox => GetBoundingBox();

        private readonly MatrixBracketsLayoutResult _brackets;
        private readonly MatrixEntriesLayoutResult _entries;
        public List<ILayoutResult> Annotations { get; set; }

        public MatrixLayoutResults(MatrixBracketsLayoutResult brackets,
            MatrixEntriesLayoutResult entries)
        {
            _brackets = brackets;
            _entries = entries;
        }

        public IEnumerable<ILayoutResults> GetComponents()
        {
            var results = new List<ILayoutResults>();
            results.Add(new LayoutResultsCollection(_brackets));
            results.Add(_entries);

            if (Annotations.Any())
                results.Add(new LayoutResultsCollection(Annotations.ToArray()));

            return results;
        }

        public IEnumerable<ILayoutResult> GetResults()
        {
            var result = _entries.GetResults().Concat(new[] { _brackets });
            if (Annotations.Any())
                result = result.Concat(Annotations);

            return result;
        }

        public void ShiftDown(float shift)
        {
            _brackets.ShiftDown(shift);
            _entries.ShiftDown(shift);
        }

        public RectangleF GetRowBoundingBox(int row)
        {
            return _entries.GetRowBoundingBox(row);
        }

        public RectangleF GetColumnBoundingBox(int column)
        {
            return _entries.GetColumnBoundingBox(column);
        }

        private RectangleF GetBoundingBox()
        {
            return _brackets.Bounds;
        }
    }
}
