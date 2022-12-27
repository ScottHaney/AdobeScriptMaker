using AdobeComponents.Effects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Components
{
    public abstract class IAdobeLayerComponent
    {
        public bool AddInNewLayer { get; set; } = true;
    }

    public interface IAdobeSupportsMaskComponent
    {
        public AdobeMaskComponent Mask { get; set; }
    }

    public interface IAdobeSupportsScribbleEffect
    {
        public AdobeScribbleEffect ScribbleEffect { get; set; }
    }

    public interface IAdobeSupportsTrimPathsEffect
    {
        public AdobeTrimPathsEffect TrimPathsEffect { get; set; }
    }
}
