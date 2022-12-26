﻿using System;
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
}
