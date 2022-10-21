using AdobeScriptMaker.Core.Components;
using AdobeScriptMaker.Core.Components.Layers;
using DirectRendering;
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
            var layers = new List<AdobeShapeLayer>();
            foreach (var item in drawingSequence.Drawings)
            {
                var layer = new AdobeShapeLayer(
                    new AdobePathComponent(item.Points.Select(x => new PointF(x.X, x.Y)).ToArray()) { Thickness = item.Thickness });

                layers.Add(layer);
            }

            var defaultComp = new AdobeComposition(layers.ToArray());
            return new AdobeScript(defaultComp);
        }
    }
}
