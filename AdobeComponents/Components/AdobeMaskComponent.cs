using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Components
{
    public class AdobeMaskComponent
    {
        public readonly AdobePathComponent PathComponent;

        public bool IsInverted { get; set; }

        public string MaskName { get; set; }

        public AdobeMaskComponent(AdobePathComponent pathComponent)
        {
            PathComponent = pathComponent;
        }
    }
}
