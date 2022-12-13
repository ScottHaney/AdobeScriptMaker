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
            return $".setValue({_scriptText})";
        }
    }

    public class AdobeColorControlRef : IAdobeColorValue
    {
        private readonly string _compRef;
        private readonly string _layerName;
        private readonly string _colorControlName;

        public AdobeColorControlRef(string compRef,
            string layerName,
            string colorControlName)
        {
            _compRef = compRef;
            _layerName = layerName;
            _colorControlName = colorControlName;
        }

        public string GetScriptText()
        {
            return $".expression = \"{_compRef}.layer('{_layerName}').effect('{_colorControlName}')('Color')\"";
        }
    }
}
