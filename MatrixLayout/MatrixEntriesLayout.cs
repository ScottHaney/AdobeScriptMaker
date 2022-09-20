using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace MatrixLayout
{
    public class MatrixEntriesLayout
    {
        public readonly float OuterPaddingPercentage;
        public readonly float RowGapPercentage;
        public readonly float ColumnGapPercentage;

        public readonly int Rows;
        public readonly int Columns;

        public MatrixEntriesLayout(float outerPaddingPercentage,
            float rowGapPercentage,
            float columnGapPercentage,
            int rows,
            int columns)
        {
            OuterPaddingPercentage = outerPaddingPercentage;
            RowGapPercentage = rowGapPercentage;
            ColumnGapPercentage = columnGapPercentage;

            Rows = rows;
            Columns = columns;
        }

        public MatrixEntriesLayoutResult GetLayoutResult(RectangleF availableSpace)
        {
            var innerWidth = (1 - 2 * OuterPaddingPercentage) * availableSpace.Width;
            var innerHeight = (1 - 2 * OuterPaddingPercentage) * availableSpace.Height;

            var rowHeight = (innerHeight - (Rows - 1) * RowGapPercentage * availableSpace.Height) / Rows;
            var colWidth = (innerWidth - (Columns - 1) * ColumnGapPercentage * availableSpace.Width) / Columns;

            var leftX = availableSpace.Left + (availableSpace.Width * OuterPaddingPercentage);
            var topY = availableSpace.Top + (availableSpace.Height * OuterPaddingPercentage);

            var results = new List<RectangleF>();
            for (int rowIndex = 0; rowIndex < Rows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Columns; columnIndex++)
                {
                    var left = leftX + (columnIndex * colWidth) + (columnIndex * ColumnGapPercentage * availableSpace.Width);
                    var top = topY + (rowIndex * rowHeight) + (rowIndex * RowGapPercentage * availableSpace.Height);

                    var rect = new RectangleF(left, top, colWidth, rowHeight);
                    results.Add(rect);
                }
            }

            return new MatrixEntriesLayoutResult(results, Columns);
        }
    }

    public class MatrixEntriesLayoutResult
    {
        private readonly IList<RectangleF> _results;
        private readonly int _columns;
        
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
    }
}
