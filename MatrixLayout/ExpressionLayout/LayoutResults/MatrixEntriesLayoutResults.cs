using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class MatrixEntriesLayoutResult : ILayoutResults
    {
        private readonly IList<RectangleF> _results;
        private readonly int _columns;

        public IEnumerable<RectangleF> Results => new ReadOnlyCollection<RectangleF>(_results);

        public RectangleF BoundingBox => GetBoundingBox();

        public MatrixEntriesLayoutResult(IList<RectangleF> results, int columns)
        {
            _results = results;
            _columns = columns;
        }

        public RectangleF GetEntryBounds(int rowIndex, int columnIndex)
        {
            var entryIndex = columnIndex + (rowIndex * _columns);
            return _results[entryIndex];
        }

        public IEnumerable<ILayoutResult> GetResults()
        {
            return _results.Select(x => new MatrixEntryLayoutResult(x));
        }

        private RectangleF GetBoundingBox()
        {
            var topLeft = GetEntryBounds(0, 0);
            var bottomRight = GetEntryBounds(_results.Count / _columns, _columns - 1);

            return new RectangleF(topLeft.Left,
                topLeft.Top,
                bottomRight.Right - topLeft.Left,
                topLeft.Top - bottomRight.Bottom);
        }
    }

    public class MatrixEntryLayoutResult : ILayoutResult
    {
        public RectangleF Bounds { get; private set; }

        public MatrixEntryLayoutResult(RectangleF bounds)
        {
            Bounds = bounds;
        }
    }
}
