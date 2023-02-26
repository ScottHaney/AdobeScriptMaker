using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions
{
    public class DigitCrossBar : IDigitChisleAction
    {
        private readonly float _lineWidthPercentage;

        public bool ExtendLeft { get; set; }
        public float RightPadding { get; set; }

        public DigitCrossBar(float lineWidthPercentage)
        {
            _lineWidthPercentage = lineWidthPercentage;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var lineHeight = _lineWidthPercentage * outerBounds.Width;
            var middleY = outerBounds.Top + outerBounds.Height / 2;

            var crossBarWidth = (outerBounds.Right - outerBounds.Left) - 2 * lineHeight;

            var leftShift = ExtendLeft ? lineHeight : 0;
            var rightShift = RightPadding * crossBarWidth;

            yield return new DigitChiselResult(new[]
            {
                new PointF(outerBounds.Left + lineHeight - leftShift, middleY - lineHeight / 2),
                new PointF(outerBounds.Right - lineHeight - rightShift, middleY - lineHeight / 2),
                new PointF(outerBounds.Right - lineHeight - rightShift, middleY + lineHeight / 2),
                new PointF(outerBounds.Left + lineHeight - leftShift, middleY + lineHeight / 2)
            }, new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(ExtendLeft ? false : true, ExtendLeft ? false : true));
        }
    }
}
