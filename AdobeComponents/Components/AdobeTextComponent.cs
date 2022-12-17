﻿using System;
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
            FontName = FontNameRemapping(fontName);
            FontSizeInPixels = (sizeInPoints * 4) / 3;
        }

        private string FontNameRemapping(string fontName)
        {
            //Sometimes there is a difference in the name used by the font
            //for the operating system and for adobe after effects so account for that here
            if (fontName == "Graphie Light")
                return "Graphie-Light";
            else
                return fontName;
        }
    }
}
