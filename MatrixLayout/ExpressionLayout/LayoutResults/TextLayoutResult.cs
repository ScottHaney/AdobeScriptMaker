using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class TextLayoutResult : ILayoutResult
    {
        public RectangleF Bounds { get; private set; }
        public TextSettings TextSettings { get; private set; }
        public string Text { get; private set; }

        public object Metadata { get; set; }

        public TextJustification Justification { get; set; } = TextJustification.Right;

        public TextLayoutResult(RectangleF bounds,
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

    public enum TextJustification
    {
        Right,
        Left
    }

    public class RowAnnotationMetadata
    {
        public readonly int Row;

        public RowAnnotationMetadata(int row)
        {
            Row = row;
        }
    }
}
