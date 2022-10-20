using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.Core.Components
{
    public class AdobeComposition
    {
        public readonly AdobePathComponent Paths;

        public AdobeComposition(AdobePathComponent paths)
        {
            Paths = paths;
        }
    }
}
