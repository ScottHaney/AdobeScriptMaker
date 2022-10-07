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

        private static readonly SizeF _bitmapSize = new SizeF(5000, 5000);

        public TextMeasurer()
        {
            _bitmap = new Bitmap((int)_bitmapSize.Width, (int)_bitmapSize.Height);
            _graphics = Graphics.FromImage(_bitmap);
        }

        public SizeF MeasureText(string text, Font font)
        {
            //This actually returns the correct width without adding extra padding
            //This code was taken from: https://stackoverflow.com/questions/26360757/c-sharp-textrenderer-measuretext-is-a-few-pixels-too-wide
            return _graphics.MeasureString(text, font, _bitmapSize,
                                                  StringFormat.GenericTypographic);
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

    public interface ITextMeasurerFactory
    {
        ITextMeasurer Create();
    }

    public class TextMeasurerFactory : ITextMeasurerFactory
    {
        public ITextMeasurer Create()
        {
            return new TextMeasurer();
        }
    }
}
