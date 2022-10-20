using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace AdobeScriptMaker.Core.Components
{
    public class AdobePathComponent
    {
        public readonly Point[] Points;

        public AdobePathComponent(params Point[] points)
        {
            Points = points ?? Array.Empty<Point>();
        }
    }
}
