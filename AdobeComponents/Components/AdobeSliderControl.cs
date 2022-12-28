using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Components
{
    public class AdobeSliderControl : IAdobeLayerComponent
    {
        public string Name { get; set; }
        public AdobeSliderControlValue[] Values { get; set; } = Array.Empty<AdobeSliderControlValue>();
    }

    public class AdobeSliderControlValue
    {
        public double Time { get; set; }
        public double Value { get; set; }
    }
}
