using AdobeScriptMaker.Core.Components;
using AdobeScriptMaker.Core.Components.Layers;
using DirectRendering;
using DirectRendering.Drawing;
using DirectRendering.Drawing.Animation;
using DirectRendering.Text;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdobeScriptMaker.Core.ComponentsConverters
{
    public class AdobeComponentsConverter
    {
        public AdobeScript Convert(DrawingSequence drawingSequence)
        {
            var layers = new List<AdobeLayer>();

            double currentTime = 0;
            foreach (var context in drawingSequence.Contexts)
            {
                var startTime = context.StartTime.GetAbsoluteTime(currentTime);
                var endTime = startTime + context.Duration.GetAbsoluteTime(startTime);

                var layer = new AdobeLayer(context.Drawings
                    .Select(x => Create((dynamic)x))
                    .Cast<IAdobeLayerComponent>()
                    .ToArray())
                {
                    InPoint = startTime,
                    OutPoint = endTime
                };

                layers.Add(layer);
                currentTime = endTime;
            }

            var defaultComp = new AdobeComposition(layers.ToArray());
            return new AdobeScript(defaultComp);
        }

        private AdobePathComponent Create(PathDrawing path)
        {
            return new AdobePathComponent(path.Points)
            {
                Thickness = path.Thickness,
                IsClosed = path.IsClosed,
                HasLockedScale = path.HasLockedScale
            };
        }

        private AdobeTextControl Create(SequenceDrawing path)
        {
            var values = new List<AdobeTextControlValue>()
            {
                new AdobeTextControlValue()
                {
                    Time = path.StartTime,
                    Value = path.StartText
                }
            };

            var currentText = path.StartText;
            
            foreach (var value in path.Values)
            {
                var textValueToAdd = value.Value.ToString();

                if (textValueToAdd != String.Empty)
                {
                    if (currentText == path.StartText)
                    {
                        if (path.StartText != String.Empty)
                            currentText += " ";

                        currentText += textValueToAdd;
                    }
                    else
                        currentText += ", " + textValueToAdd;
                }
                
                values.Add(new AdobeTextControlValue() { Time = value.Time, Value = currentText });
            }

            return new AdobeTextControl()
            {
                Values = values.ToArray()
            };
        }

        private AdobeSliderControl Create(SliderControl slider)
        {
            return new AdobeSliderControl()
            {
                Name = slider.Name,
                Values = slider.Values
                    .Select(x => new AdobeSliderControlValue() { Time = x.Time, Value = x.Value })
                    .ToArray()
            };
        }
    }
}
