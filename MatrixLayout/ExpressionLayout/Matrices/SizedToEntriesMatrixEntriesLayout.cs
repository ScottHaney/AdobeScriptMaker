﻿using MatrixLayout.ExpressionLayout.LayoutResults;
using MatrixLayout.InputDescriptions;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.ExpressionLayout.Matrices
{
    public class SizedToEntriesMatrixEntriesLayout : IMatrixEntriesLayout
    {
        private readonly MatrixInteriorMarginsDescription _marginsSettings;

        public readonly int Rows;
        public readonly int Columns;

        public SizedToEntriesMatrixEntriesLayout(MatrixInteriorMarginsDescription marginsSettings,
            int rows,
            int columns)
        {
            _marginsSettings = marginsSettings;

            Rows = rows;
            Columns = columns;
        }

        public MatrixEntriesLayoutResult GetLayoutResult(IMatrixEntriesLayoutInputParams inputParams, float startingLeft = 0)
        {
            var inputs = (SizedMatrixEntriesLayoutInputParams)inputParams;

            var relativeSizeValue = inputs.TextMeasurer.MeasureText("0", inputs.Font).Height;

            var sizes = inputs.Entries.Select(x => inputs.TextMeasurer.MeasureText(x, inputs.Font)).ToList();

            var combiner = new MatrixEntriesSizeCombiner();
            var columnWidths = combiner.GetMaxForEachColumn(sizes.Select(x => x.Width), Columns);
            var rowHeights = combiner.GetMaxForEachRow(sizes.Select(x => x.Height), Columns);

            var rowGap = _marginsSettings.RowGapPercentage * relativeSizeValue;
            var columnGap = _marginsSettings.ColumnGapPercentage * relativeSizeValue;

            var leftX = startingLeft + _marginsSettings.EntriesPaddingPercentageHorizontal * relativeSizeValue;
            var topY = 0;

            var results = new List<MatrixEntryLayoutResult>();
            for (int rowIndex = 0; rowIndex < Rows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Columns; columnIndex++)
                {
                    var left = leftX + columnWidths.Take(columnIndex).DefaultIfEmpty(0).Sum() + (columnIndex * columnGap);
                    var top = topY + rowHeights.Take(rowIndex).DefaultIfEmpty(0).Sum() + (rowIndex * rowGap);

                    var rect = new RectangleF(left, top, columnWidths[columnIndex], rowHeights[rowIndex]);
                    results.Add(new MatrixEntryLayoutResult(rect,
                        new TextSettings(inputs.Font),
                        inputs.Entries[rowIndex * Columns + columnIndex],
                        rowIndex,
                        columnIndex));
                }
            }

            return new MatrixEntriesLayoutResult(results, Columns, 0);
        }

        public ILayoutResults GetLayoutResultWithBrackets(IMatrixEntriesLayoutInputParams inputParams, MatrixBracketsDescription bracketsSettings, float startingLeft = 0)
        {
            var inputs = (SizedMatrixEntriesLayoutInputParams)inputParams;

            var originalResult = GetLayoutResult(inputParams, startingLeft);
            var updatedEntries = originalResult.Results
                .Select(x => new MatrixEntryLayoutResult(
                    new RectangleF(x.Bounds.Left + bracketsSettings.Thickness, x.Bounds.Top, x.Bounds.Width, x.Bounds.Height),
                    new TextSettings(inputs.Font),
                    x.Text,
                    x.Row,
                    x.Column))
                .ToList();

            var relativeSizeValue = inputs.TextMeasurer.MeasureText("0", inputs.Font).Height;

            var originalBounds = originalResult.BoundingBox;
            var outerBounds = new RectangleF(
                originalBounds.Left - _marginsSettings.EntriesPaddingPercentageHorizontal * relativeSizeValue,
                originalBounds.Top - _marginsSettings.EntriesPaddingPercentageVertical * relativeSizeValue,
                originalBounds.Width + 2 * bracketsSettings.Thickness + 2 * _marginsSettings.EntriesPaddingPercentageHorizontal * relativeSizeValue,
                originalBounds.Height + 2 * bracketsSettings.Thickness + 2 * _marginsSettings.EntriesPaddingPercentageVertical * relativeSizeValue);

            var bracketsResult = new MatrixBracketsLayoutResult(
                outerBounds,
                bracketsSettings);

            return new MatrixLayoutResults(bracketsResult,
                new MatrixEntriesLayoutResult(updatedEntries, Columns, bracketsSettings.Thickness));
        }
    }

    public class SizedMatrixEntriesLayoutInputParams: IMatrixEntriesLayoutInputParams
    {
        public readonly ITextMeasurer TextMeasurer;
        public readonly Font Font;
        public readonly string[] Entries;

        public SizedMatrixEntriesLayoutInputParams(ITextMeasurer textMeasurer,
            Font font,
            params string[] entries)
        {
            TextMeasurer = textMeasurer;
            Font = font;
            Entries = entries;
        }

        public SizedMatrixEntriesLayoutInputParams(ITextMeasurer textMeasurer,
            Font font,
            params double[] entries)
        {
            TextMeasurer = textMeasurer;
            Font = font;
            Entries = entries.Select(x => x.ToString()).ToArray();
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
