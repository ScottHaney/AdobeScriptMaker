using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Components
{
    public class AdobePathGroupComponent : IAdobeLayerComponent
    {
        public readonly AdobePathComponent[] Paths;

        public AdobePathGroupComponent(params AdobePathComponent[] paths)
        {
            Paths = paths ?? Array.Empty<AdobePathComponent>();
        }
    }
}
