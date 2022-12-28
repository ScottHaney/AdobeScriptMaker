using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What
{
    public class AreaUnderFunctionRenderingDescription : IWhatToRender
    {
        public readonly FunctionRenderingDescription FunctionRenderingDescription;

        public AreaUnderFunctionRenderingDescription(string uniqueName,
            FunctionRenderingDescription functionRenderingDescription)
            : base(uniqueName)
        {
            FunctionRenderingDescription = functionRenderingDescription;
        }
    }
}
