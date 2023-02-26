using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions
{
    public class DigitCorner : IDigitChisleAction
    {
        private readonly DigitCornerName _cornerName;
        private readonly float _widthPercentage;
        private readonly float _angle;

        public bool MoveToCenter { get; set; }

        public DigitCorner(DigitCornerName cornerName,
            float widthPercentage,
            float angle)
        {
            _cornerName = cornerName;
            _widthPercentage = widthPercentage;
            _angle = angle;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var result = GetResultInternal(_cornerName, outerBounds);

            if (MoveToCenter)
            {
                var centerBarHeight = _widthPercentage * outerBounds.Width;
                var distance = outerBounds.Height / 2 - centerBarHeight / 2;

                var cornerPoint = result.Points.Skip(1).First();
                var shift = cornerPoint.Y == outerBounds.Top
                    ? distance
                    : -distance;

                yield return result.ShiftY(shift);
            }
            else
                yield return result;
        }

        private DigitChiselResult GetResultInternal(DigitCornerName cornerName, RectangleF outerBounds)
        {
            var xLength = outerBounds.Width * _widthPercentage;
            var slope = (float)Math.Tan(_angle * (Math.PI / 180));

            if (cornerName == DigitCornerName.TopLeft)
            {
                var topLeft = outerBounds.TopLeft();

                var refPoint = new PointF(topLeft.X + xLength, topLeft.Y);
                var intersectionPoint = new PointF(topLeft.X, topLeft.Y + xLength * slope);

                var shadowSides = MoveToCenter
                    ? new[] { new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, true) }
                    : new[] { new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, true) };

                return new DigitChiselResult(new PointF[]
                {
                    intersectionPoint,
                    topLeft,
                    refPoint
                }, shadowSides);
            }
            else if (cornerName == DigitCornerName.TopRight)
            {
                var topRight = outerBounds.TopRight();

                var refPoint = new PointF(topRight.X - xLength, topRight.Y);
                var intersectionPoint = new PointF(topRight.X, topRight.Y + xLength * slope);

                var shadowSides = MoveToCenter
                    ? new[] { new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, MarbleOrientations.Negative) }
                    : new[] { new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, MarbleOrientations.Negative) };

                return new DigitChiselResult(new PointF[]
                {
                    refPoint,
                    topRight,
                    intersectionPoint
                }, shadowSides);
            }
            else if (cornerName == DigitCornerName.BottomRight)
            {
                var bottomRight = outerBounds.BottomRight();

                var refPoint = new PointF(bottomRight.X - xLength, bottomRight.Y);
                var intersectionPoint = new PointF(bottomRight.X, bottomRight.Y - xLength * slope);

                var shadowSides = MoveToCenter
                    ? new[] { new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(true, true) }
                    : new[] { new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(true, true) };

                return new DigitChiselResult(new PointF[]
                {
                    intersectionPoint,
                    bottomRight,
                    refPoint
                }, shadowSides);
            }
            else if (cornerName == DigitCornerName.BottomLeft)
            {
                var bottomLeft = outerBounds.BottomLeft();

                var refPoint = new PointF(bottomLeft.X + xLength, bottomLeft.Y);
                var intersectionPoint = new PointF(bottomLeft.X, bottomLeft.Y - xLength * slope);

                var shadowSides = MoveToCenter
                    ? new[] { new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, true) }
                    : new[] { new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, true) };

                return new DigitChiselResult(new PointF[]
                {
                    refPoint,
                    bottomLeft,
                    intersectionPoint
                }, shadowSides);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }

    public enum DigitCornerName
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft
    }

    public static class DigitCornerNameExtensions
    {
        public static bool IsOnRight(this DigitCornerName cornerName)
        {
            return cornerName == DigitCornerName.TopRight && cornerName == DigitCornerName.BottomRight;
        }

        public static bool IsOnLeft(this DigitCornerName cornerName)
        {
            return cornerName == DigitCornerName.TopLeft && cornerName == DigitCornerName.BottomLeft;
        }
    }
}
