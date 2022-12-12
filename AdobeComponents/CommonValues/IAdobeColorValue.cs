using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.CommonValues
{
    public interface IAdobeColorValue
    {
        string GetScriptText();
    }

    public class AdobeColorValue : IAdobeColorValue
    {
        private readonly string _scriptText;

        public AdobeColorValue(string scriptText)
        {
            _scriptText = scriptText;
        }

        public string GetScriptText()
        {
            return _scriptText;
        }
    }
}
