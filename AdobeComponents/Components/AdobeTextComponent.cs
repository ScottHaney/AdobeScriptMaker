using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AdobeComponents.Components
{
    public class AdobeTextComponent : IAdobeLayerComponent
    {
        public readonly string TextValue;
        public readonly RectangleF Bounds;
        public readonly AdobeTextSettings TextSettings;

        public AdobeTextComponent(string textValue,
            RectangleF bounds,
            AdobeTextSettings textSettings)
        {
            TextValue = textValue;
            Bounds = bounds;
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
            FontName = fontName;
            FontSizeInPixels = (sizeInPoints * 4) / 3;
        }
    }
}
