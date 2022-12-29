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
        public readonly float Value;

        public AdobeSliderValue(float value)
        {
            Value = value;
        }

        public string GetScriptText()
        {
            return $"{Value}";
        }
    }

    public class AdobeSliderControlRef : IAdobeSliderValue
    {
        private readonly float _startValue;
        private readonly string _compRef;
        private readonly string _layerName;
        private readonly string _colorControlName;

        public float SliderMult { get; set; } = 1;

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
            return $"{_startValue} + {SliderMult} * {_compRef}.layer('{_layerName}').effect('{_colorControlName}')('Slider')";
        }
    }
}
