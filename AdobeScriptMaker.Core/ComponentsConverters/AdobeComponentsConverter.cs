using AdobeScriptMaker.Core.Components;
using AdobeScriptMaker.Core.Components.Layers;
using DirectRendering;
using DirectRendering.Drawing;
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
                var layer = new AdobeShapeLayer(item.GetDrawings().Select(x => CreatePath((PathDrawing)x)).ToArray());
                layers.Add(layer);
            }

            var defaultComp = new AdobeComposition(layers.ToArray());
            return new AdobeScript(defaultComp);
        }

        private AdobePathComponent CreatePath(IDrawing drawing)
        {
            var path = drawing as PathDrawing;
            if (path == null)
                throw new NotSupportedException();

            return new AdobePathComponent(path.Points.Select(x => new PointF(x.X, x.Y)).ToArray())
            {
                Thickness = path.Thickness,
                IsClosed = path.IsClosed,
                HasLockedScale = path.HasLockedScale
            };
        }
    }
}
