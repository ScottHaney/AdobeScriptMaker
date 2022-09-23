using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.InputDescriptions
{
    public class TextDisplayDescription
    {
        public readonly string FontName;
        public readonly int FontSize;

        public TextDisplayDescription(string fontName, int fontSize)
        {
            FontName = fontName;
            FontSize = fontSize;
        }
    }
}
