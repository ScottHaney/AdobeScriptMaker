using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AdobeScriptMaker.Core.Components
{
    public class AdobePathComponent
    {
        public readonly PointF[] Points;
        public float Thickness { get; set; } = 2;
        public bool IsClosed { get; set; }
        public bool HasLockedScale { get; set; } = true;

        public AdobePathComponent(params PointF[] points)
        {
            Points = points ?? Array.Empty<PointF>();
        }
    }
}
