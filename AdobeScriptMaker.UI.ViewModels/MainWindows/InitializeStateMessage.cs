using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.UI.Core.MainWindows
{
    public class InitializeStateMessage
    {
        public readonly double Width;
        public readonly double Position;

        public InitializeStateMessage(double width,
            double position)
        {
            Width = width;
            Position = position;
        }
    }
}
