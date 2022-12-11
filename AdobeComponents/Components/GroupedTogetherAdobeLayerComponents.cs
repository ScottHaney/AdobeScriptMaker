using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.Components
{
    public class GroupedTogetherAdobeLayerComponents : IAdobeLayerComponent
    {
        public IAdobeLayerComponent[] Components;

        public GroupedTogetherAdobeLayerComponents(params IAdobeLayerComponent[] components)
        {
            Components = components ?? Array.Empty<IAdobeLayerComponent>();
        }
    }
}
