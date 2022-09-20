using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

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
            var stringSize = _graphics.MeasureString(text, font);
            return stringSize;
        }

        public void Dispose()
        {
            _bitmap.Dispose();
            _graphics.Dispose();
        }
    }

    public interface ITextMeasurer
    {
        SizeF MeasureText(string text, Font font);
    }
}
