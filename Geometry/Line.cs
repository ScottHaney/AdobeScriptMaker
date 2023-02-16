using System;
using System.Drawing;

namespace Geometry
{
    public class Line
    {
        public readonly PointF Start;
        public readonly PointF End;

        public Line(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }

        public float? GetSlope()
        {
            if (Start.X == End.X)
                return null;
            else
            {
                return (End.Y - Start.Y) / (End.X - Start.X);
            }
        }

        public float? GetParametericValue(PointF targetPoint)
        {
            var slope = GetSlope();

            if (slope == null)
            {
                if (Start.X == targetPoint.X)
                    return targetPoint.Y - Start.Y;
                else
                    return null;
            }
            else
            {
                var xDiff = targetPoint.X - Start.X;
                var expectedYValue = Start.Y + slope * xDiff;

                if (expectedYValue == targetPoint.Y)
                    return xDiff;
                else
                    return null;
            }
        }
    }
}
