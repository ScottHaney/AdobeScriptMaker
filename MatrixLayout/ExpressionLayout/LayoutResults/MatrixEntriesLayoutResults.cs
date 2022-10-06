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
        private readonly IList<MatrixEntryLayoutResult> _results;
        private readonly int _columns;
        private readonly float _bracketThickness;

        public IEnumerable<MatrixEntryLayoutResult> Results => new ReadOnlyCollection<MatrixEntryLayoutResult>(_results);

        public RectangleF BoundingBox => GetBoundingBox();

        public MatrixEntriesLayoutResult(IList<MatrixEntryLayoutResult> results, int columns, float bracketThickness)
        {
            _results = results;
            _columns = columns;
            _bracketThickness = bracketThickness;
        }

        public RectangleF GetEntryBounds(int rowIndex, int columnIndex)
        {
            var entryIndex = columnIndex + (rowIndex * _columns);
            return _results[entryIndex].Bounds;
        }

        public RectangleF GetRowBoundingBox(int row)
        {
            var rowBoxes = new List<RectangleF>();
            for (var i = 0; i < _columns; i++)
                rowBoxes.Add(_results[row * _columns + i].Bounds);

            var left = rowBoxes.Min(x => x.Left);
            var right = rowBoxes.Max(x => x.Right);
            var top = rowBoxes.Min(y => y.Top);
            var bottom = rowBoxes.Max(y => y.Bottom);

            return new RectangleF(left, top, right - left, bottom - top);
        }

        public RectangleF GetColumnBoundingBox(int column)
        {
            var numRows = _results.Count / _columns;

            var columnBoxes = new List<RectangleF>();
            for (var i = column; i < _results.Count; i += numRows)
                columnBoxes.Add(_results[i].Bounds);

            var left = columnBoxes.Min(x => x.Left);
            var right = columnBoxes.Max(x => x.Right);
            var top = columnBoxes.Min(y => y.Top);
            var bottom = columnBoxes.Max(y => y.Bottom);

            return new RectangleF(left, top, right - left, bottom - top);
        }

        public IEnumerable<ILayoutResult> GetResults()
        {
            return Results;
        }

        public void ShiftDown(float diff)
        {
            foreach (var result in _results)
                result.ShiftDown(diff);
        }

        public IEnumerable<ILayoutResults> GetComponents()
        {
            yield return this;
        }

        private RectangleF GetBoundingBox()
        {
            var topLeft = GetEntryBounds(0, 0);
            var bottomRight = GetEntryBounds(_results.Count / _columns - 1, _columns - 1);

            return new RectangleF(topLeft.Left - _bracketThickness,
                topLeft.Top - _bracketThickness,
                bottomRight.Right - topLeft.Left + 2 * _bracketThickness,
                bottomRight.Bottom - topLeft.Top + 2 * _bracketThickness);
        }
    }

    public class MatrixEntryLayoutResult : ILayoutResult
    {
        public RectangleF Bounds { get; private set; }
        public TextSettings TextSettings { get; private set; }
        public string Text { get; private set; }

        public MatrixEntryLayoutResult(RectangleF bounds,
            TextSettings textSettings,
            string text)
        {
            Bounds = bounds;
            TextSettings = textSettings;
            Text = text;
        }

        public void ShiftDown(float shift)
        {
            Bounds = new RectangleF(Bounds.Left,
                Bounds.Top + shift,
                Bounds.Width,
                Bounds.Height);
        }
    }

    public class TextSettings
    {
        public readonly string FontName;
        public readonly float FontSizeInPixels;

        public TextSettings(Font font)
        {
            FontName = font.Name;
            FontSizeInPixels = (font.SizeInPoints * 4) / 3;
        }
    }
}
