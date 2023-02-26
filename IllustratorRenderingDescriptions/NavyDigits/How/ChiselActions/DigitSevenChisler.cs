using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions
{
    public class DigitSevenChisler : IDigitChisleAction
    {
        private readonly float _bottomBoxPercentage;
        private readonly float _slantLineWidthPercentage;

        public DigitSevenChisler(float bottomBoxPercentage,
            float slantLineWidthPercentage)
        {
            _bottomBoxPercentage = bottomBoxPercentage;
            _slantLineWidthPercentage = slantLineWidthPercentage;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var bottomBoxHeight = _bottomBoxPercentage * outerBounds.Height;
            var bottomBox = new RectangleF(outerBounds.Left, outerBounds.Bottom - bottomBoxHeight, outerBounds.Width, bottomBoxHeight);

            var slantLineWidth = _slantLineWidthPercentage * outerBounds.Width;

            var rightSlantLineBottomPoint = new PointF(bottomBox.Left + slantLineWidth, bottomBox.Bottom);
            var rightSlantLineTopPoint = bottomBox.TopRight();

            var leftSlantLineBottomPoint = bottomBox.BottomLeft();
            var leftSlantLineTopPoint = new PointF(bottomBox.Right - slantLineWidth, bottomBox.Top);

            var bottomLeftCutout = new[]
            {
                bottomBox.TopLeft(),
                leftSlantLineTopPoint,
                leftSlantLineBottomPoint
            };
            yield return new DigitChiselResult(bottomLeftCutout, new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, false));

            var bottomRightCutout = new[]
            {
                rightSlantLineBottomPoint,
                rightSlantLineTopPoint,
                bottomBox.BottomRight()
            };
            yield return new DigitChiselResult(bottomRightCutout, new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false));

        }
    }
}
