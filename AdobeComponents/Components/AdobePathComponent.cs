﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AdobeComponents.Animation;
using AdobeComponents.CommonValues;
using AdobeComponents.Effects;

namespace AdobeComponents.Components
{
    public class AdobePathComponent : IAdobeSupportsMaskComponent, IAdobeSupportsScribbleEffect, IAdobeSupportsTrimPathsEffect
    {
        public readonly IAnimatedValue<PointF[]> Points;
        public float Thickness { get; set; } = 2;
        public bool IsClosed { get; set; }
        public bool HasLockedScale { get; set; } = true;

        public IAdobeColorValue ColorValue { get; set; }

        public AdobeMaskComponent Mask { get; set; }

        public AdobeScribbleEffect ScribbleEffect { get; set; }

        public AdobeTrimPathsEffect TrimPathsEffect { get; set; }

        public IAdobeSliderValue StrokeWidth { get; set; } = new AdobeSliderValue(2);

        public AdobePathComponent(IAnimatedValue<PointF[]> points)
        {
            Points = points;
        }
    }
}
