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

        public FullMatrixLayoutResult GetLayoutResult(IMatrixEntriesLayoutInputParams inputParams)
        {
            var entriesLayoutResult = _entriesLayout.GetLayoutResult(inputParams);
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
            var innerBounds = _entriesResult.GetEntryBounds(rowIndex, columnIndex);

            return new RectangleF(innerBounds.Left + _bracketThickness,
                innerBounds.Top + _bracketThickness,
                innerBounds.Width,
                innerBounds.Height);
        }
    }
}
