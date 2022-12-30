using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;

namespace RenderingDescriptions.What
{
    public class TextSettings
    {
        public readonly string FontName;
        public readonly float FontSize;
        public readonly TextSettingsFontSizeUnit FontSizeUnits;

        public float FontSizeInPixels => GetFontSizeInPixels();

        public TextSettings(string fontName,
            float fontSize,
            TextSettingsFontSizeUnit fontSizeUnits)
        {
            FontName = fontName;
            FontSize = fontSize;
            FontSizeUnits = fontSizeUnits;
        }

        public TextSettings(Font font)
        {
            FontName = font.Name;
            FontSize = font.SizeInPoints;
            FontSizeUnits = TextSettingsFontSizeUnit.Points;
        }

        public Font ToFont()
        {
            return new Font(FontName, FontSize, ToGraphicsUnit(FontSizeUnits));
        }

        private float GetFontSizeInPixels()
        {
            if (FontSizeUnits == TextSettingsFontSizeUnit.Pixels)
                return FontSize;
            else if (FontSizeUnits == TextSettingsFontSizeUnit.Points)
                return (4 * FontSize) / 3f;
            else
                throw new NotSupportedException();
        }

        private GraphicsUnit ToGraphicsUnit(TextSettingsFontSizeUnit unit)
        {
            if (unit == TextSettingsFontSizeUnit.Pixels)
                return GraphicsUnit.Pixel;
            else if (unit == TextSettingsFontSizeUnit.Points)
                return GraphicsUnit.Point;
            else
                throw new NotSupportedException();
        }
    }

    public enum TextSettingsFontSizeUnit
    {
        Pixels,
        Points
    }
}
