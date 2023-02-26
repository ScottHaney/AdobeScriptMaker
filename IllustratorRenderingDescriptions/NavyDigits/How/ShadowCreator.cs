using Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How
{
    public class ShadowCreator
    {
        private readonly float _dimensionPercentage;
        private readonly float _shadowAngle;

        public ShadowCreator(float dimensionPercentage, float shadowAngle)
        {
            _dimensionPercentage = dimensionPercentage;
            _shadowAngle = shadowAngle;
        }

        public PointF[] CreateShadowPath(Line shadowLine, RectangleF marble)
        {
            double shadowDimension = _dimensionPercentage * marble.Width;

            var offsetX = shadowDimension * Math.Cos(_shadowAngle * (Math.PI / 180));
            var offsetY = shadowDimension * Math.Sin(_shadowAngle * (Math.PI / 180));


            return new PointF[] { shadowLine.Start, shadowLine.End, new PointF((float)(shadowLine.End.X + offsetX), (float)(shadowLine.End.Y + offsetY)), new PointF((float)(shadowLine.Start.X + offsetX), (float)(shadowLine.Start.Y + offsetY)) };
        }

        public PointF[] CreateAntiShadowPath(Line removeShadowLine, RectangleF marble)
        {
            var shadowAngle = -1 * _shadowAngle;

            double shadowDimension = _dimensionPercentage * marble.Width;

            var offsetX = shadowDimension * Math.Cos(shadowAngle * (Math.PI / 180));
            var offsetY = shadowDimension * Math.Sin(shadowAngle * (Math.PI / 180));


            return new PointF[] { removeShadowLine.Start, removeShadowLine.End, new PointF((float)(removeShadowLine.End.X + offsetX), (float)(removeShadowLine.End.Y + offsetY)), new PointF((float)(removeShadowLine.Start.X + offsetX), (float)(removeShadowLine.Start.Y + offsetY)) };
        }

        public PointD[] CreateShadowPathD(Line shadowLine, RectangleF marble, float forceOffsetYAbsValue)
        {
            double shadowDimension = forceOffsetYAbsValue / Math.Sin(_shadowAngle * (Math.PI / 180));

            var offsetX = shadowDimension * Math.Cos(_shadowAngle * (Math.PI / 180));
            var offsetY = shadowDimension * Math.Sin(_shadowAngle * (Math.PI / 180));


            return new PointD[] { new PointD(shadowLine.Start), new PointD(shadowLine.End), new PointD((shadowLine.End.X + offsetX), (shadowLine.End.Y + offsetY)), new PointD((shadowLine.Start.X + offsetX), (shadowLine.Start.Y + offsetY)) };
        }

        public PointF[] CreateShadowPathF(Line shadowLine, RectangleF marble, float forceOffsetYAbsValue)
        {
            double shadowDimension = forceOffsetYAbsValue / Math.Sin(_shadowAngle * (Math.PI / 180));

            var offsetX = shadowDimension * Math.Cos(_shadowAngle * (Math.PI / 180));
            var offsetY = shadowDimension * Math.Sin(_shadowAngle * (Math.PI / 180));


            return new PointF[] { shadowLine.Start, shadowLine.End, new PointF((float)(shadowLine.End.X + offsetX), (float)(shadowLine.End.Y + offsetY)), new PointF((float)(shadowLine.Start.X + offsetX), (float)(shadowLine.Start.Y + offsetY)) };
        }

        public PointD[] CreateAntiShadowPathD(Line removeShadowLine, RectangleF marble, float forceOffsetYAbsValue)
        {
            var shadowPath = CreateShadowPathD(removeShadowLine, marble, forceOffsetYAbsValue);

            var line1 = new LineD(shadowPath[0], shadowPath[2]);
            var line2 = new LineD(shadowPath[1], shadowPath[3]);

            var slope1 = line1.GetSlope();

            var xDiff1 = line1.End.X - line1.Start.X;
            var newPoint1 = new PointD(line1.Start.X - xDiff1, line1.Start.Y + (-xDiff1) * slope1.Value);

            var slope2 = line2.GetSlope();

            var xDiff2 = line2.End.X - line2.Start.X;
            var newPoint2 = new PointD(line2.Start.X - xDiff2, line2.Start.Y + (-xDiff2) * slope2.Value);

            return new PointD[] { line1.Start, line2.Start, newPoint2, newPoint1 };
        }
    }
}
