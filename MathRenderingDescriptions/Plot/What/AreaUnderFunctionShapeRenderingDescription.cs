using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathRenderingDescriptions.Plot.What
{
    public class AreaUnderFunctionShapeRenderingDescription : IWhatToRender
    {
        public readonly FunctionRenderingDescription FunctionRenderingDescription;

        public AreaUnderFunctionShapeRenderingDescription(string uniqueName,
            FunctionRenderingDescription functionRenderingDescription)
            : base(uniqueName)
        {
            FunctionRenderingDescription = functionRenderingDescription;
        }
    }
}
