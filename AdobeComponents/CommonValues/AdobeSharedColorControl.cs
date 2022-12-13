using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.CommonValues
{
    public class AdobeSharedColorControl : IAdobeSharedValueControl
    {
        public readonly string ControlName;

        public AdobeSharedColorControl(string controlName)
        {
            ControlName = controlName;
        }
    }
}
