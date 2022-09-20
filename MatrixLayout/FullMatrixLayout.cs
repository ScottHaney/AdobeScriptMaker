using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout
{
    public class FullMatrixLayout
    {
        private readonly UniformlySizedMatrixEntriesLayout _entriesLayout;
        private readonly float _bracketThickness;

        public FullMatrixLayout(UniformlySizedMatrixEntriesLayout entriesLayout,
            float bracketThickness)
        {
            _entriesLayout = entriesLayout;
            _bracketThickness = bracketThickness;
        }

        public FullMatrixLayoutResult GetLayoutResult(RectangleF availableSpace)
        {
            var entriesRect = new RectangleF(availableSpace.Left + _bracketThickness,
                availableSpace.Top + _bracketThickness,
                availableSpace.Width - 2 * _bracketThickness,
                availableSpace.Height - 2 * _bracketThickness);

            var entriesLayoutResult = _entriesLayout.GetLayoutResult(entriesRect);
            return new FullMatrixLayoutResult(entriesLayoutResult, _bracketThickness);
        }
    }

    public class FullMatrixLayoutResult
    {
        private readonly MatrixEntriesLayoutResult _entriesResult;
        private readonly float _bracketThickness;

        public FullMatrixLayoutResult(MatrixEntriesLayoutResult entriesResult,
            float bracketThickness)
        {
            _entriesResult = entriesResult;
            _bracketThickness= bracketThickness;
        }

        public RectangleF GetEntryBounds(int rowIndex, int columnIndex)
        {
            return _entriesResult.GetEntryBounds(rowIndex, columnIndex);
        }
    }
}
