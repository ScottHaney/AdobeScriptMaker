using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MatrixLayout
{
    public class TextMeasurer : ITextMeasurer, IDisposable
    {
        private readonly Bitmap _bitmap;
        private readonly Graphics _graphics;

        public TextMeasurer()
        {
            _bitmap = new Bitmap(500, 200);
            _graphics = Graphics.FromImage(_bitmap);
        }

        public SizeF MeasureText(string text, Font font)
        {
            var stringSize = TextRenderer.MeasureText(text, font);
            return new SizeF(stringSize.Width, font.Size);
        }

        public void Dispose()
        {
            _bitmap.Dispose();
            _graphics.Dispose();
        }
    }

    public interface ITextMeasurer : IDisposable
    {
        SizeF MeasureText(string text, Font font);
    }
}
