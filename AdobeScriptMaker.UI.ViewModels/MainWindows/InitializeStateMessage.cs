using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeScriptMaker.UI.Core.MainWindows
{
    public class InitializeStateMessage
    {
        public readonly double Position;

        public InitializeStateMessage(double position)
        {
            Position = position;
        }
    }
}
