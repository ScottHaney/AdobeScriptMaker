using System;
using System.Collections.Generic;
using System.Text;

namespace AdobeComponents.CommonValues
{
    public interface IAdobeSliderValue
    {
        string GetScriptText();
    }

    public class AdobeSliderValue : IAdobeSliderValue
    {
        private readonly float _value;

        public AdobeSliderValue(float value)
        {
            _value = value;
        }

        public string GetScriptText()
        {
            return $"{_value}";
        }
    }

    public class AdobeSliderControlRef : IAdobeSliderValue
    {
        private readonly float _startValue;
        private readonly string _compRef;
        private readonly string _layerName;
        private readonly string _colorControlName;

        public AdobeSliderControlRef(float startValue,
            string compRef,
            string layerName,
            string colorControlName)
        {
            _startValue = startValue;
            _compRef = compRef;
            _layerName = layerName;
            _colorControlName = colorControlName;
        }

        public string GetScriptText()
        {
            return $"{_startValue} + {_compRef}.layer('{_layerName}').effect('{_colorControlName}')('Slider')";
        }
    }
}
