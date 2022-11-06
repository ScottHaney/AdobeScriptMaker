﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.Core.Components
{
    public class AdobeTextControl : IAdobeLayerComponent
    {
        public AdobeTextControlValue[] Values { get; set; }
    }

    public class AdobeTextControlValue
    {
        public double Time { get; set; }
        public string Value { get; set; }
    }
}
