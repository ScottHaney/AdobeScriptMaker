using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout
{
    public class TextMeasurer : ITextMeasurer
    {
        public SizeF MeasureText(string text, Font font)
        {
            Bitmap objBitmap = default(Bitmap);
            Graphics objGraphics = default(Graphics);

            objBitmap = new Bitmap(500, 200);
            objGraphics = Graphics.FromImage(objBitmap);

            SizeF stringSize = objGraphics.MeasureString(text, font);

            objBitmap.Dispose();
            objGraphics.Dispose();
            return stringSize;
        }
    }

    public interface ITextMeasurer
    {
        SizeF MeasureText(string text, Font font);
    }
}
