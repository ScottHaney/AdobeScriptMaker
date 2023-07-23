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
        AdobeMaskComponent Mask { get; set; }
    }

    public interface IAdobeSupportsScribbleEffect
    {
        AdobeScribbleEffect ScribbleEffect { get; set; }
    }

    public interface IAdobeSupportsTrimPathsEffect
    {
        AdobeTrimPathsEffect TrimPathsEffect { get; set; }
    }
}
