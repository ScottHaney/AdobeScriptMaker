using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions
{
    public class DigitOneChisler : IDigitChisleAction
    {
        private readonly float _lineWidthPercentage;

        public DigitOneChisler(float lineWidthPercentage)
        {
            _lineWidthPercentage = lineWidthPercentage;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var lineWidth = _lineWidthPercentage * outerBounds.Width;

            var leftOverSpace = outerBounds.Width - lineWidth;
            var leftCutout = new RectangleF(outerBounds.TopLeft(), new SizeF(leftOverSpace / 2, outerBounds.Height));

            yield return new DigitChiselResult(leftCutout, new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false));

            var rightCutout = new RectangleF(outerBounds.Right - leftOverSpace / 2, outerBounds.Top, leftOverSpace / 2, outerBounds.Height);
            yield return new DigitChiselResult(rightCutout, new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(true, true));
        }
    }
}
