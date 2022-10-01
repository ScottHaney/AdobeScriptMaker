using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.InputDescriptions
{
    public class TextDisplayDescription
    {
        public readonly string FontName;
        public readonly int FontSizeInPixels;

        public TextDisplayDescription(string fontName, int fontSizeInPixels)
        {
            FontName = fontName;
            FontSizeInPixels = fontSizeInPixels;
        }
    }
}
