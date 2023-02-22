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
    }
}
