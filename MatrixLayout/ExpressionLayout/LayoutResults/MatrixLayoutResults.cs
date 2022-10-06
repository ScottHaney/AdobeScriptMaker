using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class MatrixLayoutResults : ILayoutResults
    {
        public RectangleF BoundingBox => throw new NotImplementedException();

        private readonly MatrixBracketsLayoutResult _brackets;
        private readonly MatrixEntriesLayoutResult _entries;

        public MatrixLayoutResults(MatrixBracketsLayoutResult brackets,
            MatrixEntriesLayoutResult entries)
        {
            _brackets = brackets;
            _entries = entries;
        }

        public IEnumerable<ILayoutResults> GetComponents()
        {
            return new ILayoutResults[] { new LayoutResultsCollection(_brackets), _entries };
        }

        public IEnumerable<ILayoutResult> GetResults()
        {
            return _entries.GetResults().Concat(new[] { _brackets });
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
    }
}
