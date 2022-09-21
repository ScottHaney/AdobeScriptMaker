using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.ExpressionLayout.Matrices
{
    public class SizedToEntriesMatrixEntriesLayout : IMatrixEntriesLayout
    {
        public readonly float OuterPaddingPercentage;
        public readonly float RowGapPercentage;
        public readonly float ColumnGapPercentage;

        public readonly int Rows;
        public readonly int Columns;

        public SizedToEntriesMatrixEntriesLayout(float outerPaddingPercentage,
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

        public MatrixEntriesLayoutResult GetLayoutResult(IMatrixEntriesLayoutInputParams inputParams, float startingLeft = 0)
        {
            var inputs = (SizedMatrixEntriesLayoutInputParams)inputParams;

            var relativeSizeValue = inputs.TextMeasurer.MeasureText("0", inputs.Font).Height;

            var sizes = inputs.Entries.Select(x => inputs.TextMeasurer.MeasureText(x.ToString(), inputs.Font)).ToList();

            var combiner = new MatrixEntriesSizeCombiner();
            var columnWidths = combiner.GetMaxForEachColumn(sizes.Select(x => x.Width), Columns);
            var rowHeights = combiner.GetMaxForEachRow(sizes.Select(x => x.Height), Columns);

            var rowGap = RowGapPercentage * relativeSizeValue;
            var columnGap = ColumnGapPercentage * relativeSizeValue;

            var leftX = startingLeft + OuterPaddingPercentage * relativeSizeValue;
            var topY = OuterPaddingPercentage * relativeSizeValue;

            var results = new List<RectangleF>();
            for (int rowIndex = 0; rowIndex < Rows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Columns; columnIndex++)
                {
                    var left = leftX + (columnIndex * columnWidths.Take(columnIndex).DefaultIfEmpty(0).Sum()) + (columnIndex * columnGap);
                    var top = topY + (rowIndex * rowHeights.Take(rowIndex).DefaultIfEmpty(0).Sum()) + (rowIndex * rowGap);

                    var rect = new RectangleF(left, top, columnWidths[columnIndex], rowHeights[rowIndex]);
                    results.Add(rect);
                }
            }

            return new MatrixEntriesLayoutResult(results, Columns, 0);
        }

        public MatrixEntriesLayoutResult GetLayoutResultWithBrackets(IMatrixEntriesLayoutInputParams inputParams, float bracketThickness, float startingLeft = 0)
        {
            var originalResult = GetLayoutResult(inputParams, startingLeft);
            var updatedEntries = originalResult.Results
                .Select(x => new RectangleF(x.Left + bracketThickness, x.Top + bracketThickness, x.Width, x.Height))
                .ToList();

            return new MatrixEntriesLayoutResult(updatedEntries, Columns, bracketThickness);
        }
    }

    public class SizedMatrixEntriesLayoutInputParams: IMatrixEntriesLayoutInputParams
    {
        public readonly ITextMeasurer TextMeasurer;
        public readonly Font Font;
        public readonly double[] Entries;

        public SizedMatrixEntriesLayoutInputParams(ITextMeasurer textMeasurer,
            Font font,
            params double[] entries)
        {
            TextMeasurer = textMeasurer;
            Font = font;
            Entries = entries;
        }
    }

    public class MatrixEntriesSizeCombiner
    {
        public List<float> GetMaxForEachColumn(IEnumerable<float> values, int numColumns)
        {
            return values
                .Select((x, i) => new { Value = x, Index = i })
                .GroupBy(x => x.Index % numColumns)
                .OrderBy(x => x.Key)
                .Select(x => x.Max(y => y.Value))
                .ToList();
        }

        public List<float> GetMaxForEachRow(IEnumerable<float> values, int numColumns)
        {
            return values
                .Select((x, i) => new { Value = x, Index = i })
                .GroupBy(x => x.Index / numColumns)
                .OrderBy(x => x.Key)
                .Select(x => x.Max(y => y.Value))
                .ToList();
        }
    }
}
