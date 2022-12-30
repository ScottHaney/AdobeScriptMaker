using AdobeComponents.CommonValues;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdobeComponents.Components
{
    public class AdobeTextComponent : IAdobeLayerComponent, IAdobeSupportsMaskComponent
    {
        public readonly string TextValue;
        public readonly SizeF Size;
        public readonly IAdobeSliderValue Left;
        public readonly IAdobeSliderValue Top;
        public readonly AdobeTextSettings TextSettings;

        public IAdobeColorValue FontColor { get; set; }

        public AdobeMaskComponent Mask { get; set; }

        public AdobeTextComponent(string textValue,
            RectangleF bounds,
            AdobeTextSettings textSettings)
        {
            TextValue = textValue;
            Size = bounds.Size;
            Left = new AdobeSliderValue(bounds.Left);
            Top = new AdobeSliderValue(bounds.Top);
            TextSettings = textSettings;
        }

        public AdobeTextComponent(string textValue,
            SizeF size,
            IAdobeSliderValue left,
            IAdobeSliderValue top,
            AdobeTextSettings textSettings)
        {
            TextValue = textValue;
            Size = size;
            Left = left;
            Top = top;
            TextSettings = textSettings;
        }
    }

    public class AdobeTextSettings
    {
        public readonly string FontName;
        public readonly float FontSizeInPixels;

        public AdobeTextSettings(string fontName,
            float sizeInPoints)
        {
            FontName = FontNameRemapping(fontName);
            FontSizeInPixels = (sizeInPoints * 4) / 3;
        }

        private string FontNameRemapping(string fontName)
        {
            //Sometimes there is a difference in the name used by the font
            //for the operating system and for adobe after effects so account for that here
            if (fontName.StartsWith("Graphie "))
            {
                var words = fontName.Split(' ');
                return $"{words[0]}-{string.Join("", words.Skip(1))}";
            }
            else
                return fontName;
        }
    }
}
