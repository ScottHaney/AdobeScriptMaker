using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Lines
{
    public interface ISlope
    {
        double GetXValue(PointD startPoint, double targetY);
        double GetYValue(PointD startPoint, double targetX);
        double GetValue();
        ISlope GetPerpendicularSlope();
        ArcLengthInfo GetDistanceInfoForArcLength(PointD startPoint, double arcLength);
    }

    public class ArcLengthInfo
    {
        public readonly double XDiff;
        public readonly double YDiff;

        public ArcLengthInfo(double xDiff, double yDiff)
        {
            XDiff = xDiff;
            YDiff = yDiff;
        }
    }
}
