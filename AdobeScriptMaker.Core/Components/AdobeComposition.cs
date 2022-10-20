using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.Core.Components
{
    public class AdobeComposition
    {
        public readonly AdobePathComponent[] Paths;
        public string Name { get; set; }
        public bool IsDefaultComp { get; set; }

        public AdobeComposition(params AdobePathComponent[] paths)
        {
            Paths = paths ?? Array.Empty<AdobePathComponent>();
        }
    }
}
