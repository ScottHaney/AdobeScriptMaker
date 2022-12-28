using System;
using System.Collections.Generic;
using System.Text;

namespace RenderingDescriptions.What
{
    public abstract class IWhatToRender
    {
        public readonly string UniqueName;

        public IWhatToRender(string uniqueName)
        {
            UniqueName = uniqueName;
        }
    }
}
