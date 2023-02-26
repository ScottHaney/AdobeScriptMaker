using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions
{
    public class DigitFourChisler : IDigitChisleAction
    {
        private readonly float _verticalLineWidthPercentage;
        private readonly float _horizontalLineHeightPercentage;
        private readonly float _linesIntersectionXPercentage;
        private readonly float _linesIntersectionYPercentage;
        private readonly float _slantBarWidthPercentage;

        public DigitFourChisler(float verticalLineWidthPercentage,
            float horizontalLineHeightPercentage,
            float linesIntersectionXPercentage,
            float linesIntersectionYPercentage,
            float slantBarWidthPercentage)
        {
            _verticalLineWidthPercentage = verticalLineWidthPercentage;
            _horizontalLineHeightPercentage = horizontalLineHeightPercentage;
            _linesIntersectionXPercentage = linesIntersectionXPercentage;
            _linesIntersectionYPercentage = linesIntersectionYPercentage;
            _slantBarWidthPercentage = slantBarWidthPercentage;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF marble)
        {
            var verticalLineWidth = _verticalLineWidthPercentage * marble.Width;
            var horizontalLineHeight = _horizontalLineHeightPercentage * marble.Width;
            var slantBarWidth = _slantBarWidthPercentage * marble.Width;

            var intersectionX = marble.Left + _linesIntersectionXPercentage * marble.Width;
            var intersectionY = marble.Top + _linesIntersectionYPercentage * marble.Height;

            var innerRect = new RectangleF(
                intersectionX - verticalLineWidth / 2,
                intersectionY + horizontalLineHeight / 2,
                verticalLineWidth,
                horizontalLineHeight);

            var bottomLeftRect = new RectangleF(marble.Left, innerRect.Bottom, innerRect.Left - marble.Left, marble.Bottom - innerRect.Bottom);
            yield return new DigitChiselResult(bottomLeftRect, new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false));

            var bottomRightRect = new RectangleF(innerRect.Right, innerRect.Bottom, marble.Right - innerRect.Right, marble.Bottom - innerRect.Bottom);
            yield return new DigitChiselResult(bottomRightRect, new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(true, true));

            var topRightRect = new RectangleF(innerRect.Right, marble.Top, marble.Right - innerRect.Right, innerRect.Top - marble.Top);
            yield return new DigitChiselResult(topRightRect, new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(true, true));

            var topLeftRect = new RectangleF(marble.Left, marble.Top, innerRect.Left - marble.Left, innerRect.Top - marble.Top);
            var topLeftTriangle = new PointF[] { topLeftRect.BottomLeft(), topLeftRect.TopLeft(), topLeftRect.TopRight() };
            yield return new DigitChiselResult(topLeftTriangle, new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, true));

            var slope = (topLeftRect.Bottom - topLeftRect.Top) / (topLeftRect.Right - topLeftRect.Left);
            var topLeftCutoutBottomLeftPoint = new PointF(topLeftRect.Left + slantBarWidth, topLeftRect.Bottom);
            var topLeftCutoutBottomRightPoint = innerRect.TopLeft();

            var topLeftCutoutTopPoint = new PointF(topLeftCutoutBottomRightPoint.X, topLeftCutoutBottomRightPoint.Y - slope * (topLeftRect.Width - slantBarWidth));

            var topLeftCutout = new[]
            {
                topLeftCutoutBottomLeftPoint,
                topLeftCutoutTopPoint,
                topLeftCutoutBottomRightPoint
            };

            yield return new DigitChiselResult(topLeftCutout, new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, true));
        }
    }
}
