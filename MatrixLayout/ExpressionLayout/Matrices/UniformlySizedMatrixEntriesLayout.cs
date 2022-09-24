﻿using MatrixLayout.ExpressionLayout;
using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace MatrixLayout.ExpressionLayout.Matrices
{
    public class UniformlySizedMatrixEntriesLayout : IMatrixEntriesLayout
    {
        public readonly float OuterPaddingPercentage;
        public readonly float RowGapPercentage;
        public readonly float ColumnGapPercentage;

        public readonly int Rows;
        public readonly int Columns;

        public UniformlySizedMatrixEntriesLayout(float outerPaddingPercentage,
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
            => GetLayoutResultInternal(inputParams, 0, startingLeft);

        public MatrixEntriesLayoutResult GetLayoutResultWithBrackets(IMatrixEntriesLayoutInputParams inputParams, float bracketThickness, float startingLeft = 0)
        {
            var originalRect = ((UniformMatrixEntriesLayoutInputParams)inputParams).AvailableSpace;

            var updatedRect = new RectangleF(originalRect.Left + bracketThickness,
                originalRect.Top + bracketThickness,
                originalRect.Width - 2 * bracketThickness,
                originalRect.Height - 2 * bracketThickness);

            return GetLayoutResultInternal(new UniformMatrixEntriesLayoutInputParams(updatedRect), bracketThickness, startingLeft);
        }

        public MatrixEntriesLayoutResult GetLayoutResultInternal(IMatrixEntriesLayoutInputParams inputParams, float bracketThickness, float startingLeft = 0)
        {
            var inputs = (UniformMatrixEntriesLayoutInputParams)inputParams;

            var innerWidth = (1 - 2 * OuterPaddingPercentage) * inputs.AvailableSpace.Width;
            var innerHeight = (1 - 2 * OuterPaddingPercentage) * inputs.AvailableSpace.Height;

            var rowHeight = (innerHeight - (Rows - 1) * RowGapPercentage * inputs.AvailableSpace.Height) / Rows;
            var colWidth = (innerWidth - (Columns - 1) * ColumnGapPercentage * inputs.AvailableSpace.Width) / Columns;

            var leftX = startingLeft + inputs.AvailableSpace.Left + (inputs.AvailableSpace.Width * OuterPaddingPercentage);
            var topY = inputs.AvailableSpace.Top + (inputs.AvailableSpace.Height * OuterPaddingPercentage);

            var results = new List<MatrixEntryLayoutResult>();
            for (int rowIndex = 0; rowIndex < Rows; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Columns; columnIndex++)
                {
                    var left = leftX + (columnIndex * colWidth) + (columnIndex * ColumnGapPercentage * inputs.AvailableSpace.Width);
                    var top = topY + (rowIndex * rowHeight) + (rowIndex * RowGapPercentage * inputs.AvailableSpace.Height);

                    var rect = new RectangleF(left, top, colWidth, rowHeight);
                    results.Add(new MatrixEntryLayoutResult(rect, null, ""));
                }
            }

            return new MatrixEntriesLayoutResult(results, Columns, bracketThickness);
        }
    }

    public class UniformMatrixEntriesLayoutInputParams: IMatrixEntriesLayoutInputParams
    {
        public readonly RectangleF AvailableSpace;

        public UniformMatrixEntriesLayoutInputParams(RectangleF availableSpace)
        {
            AvailableSpace = availableSpace;
        }
    }

    
}
