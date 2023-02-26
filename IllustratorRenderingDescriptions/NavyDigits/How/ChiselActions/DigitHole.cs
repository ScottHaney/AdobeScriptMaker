using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions
{
    public class DigitHole : IDigitChisleAction
    {
        private readonly DigitHoleName _name;
        private readonly float _widthPaddingPercentage;

        public DigitHole(DigitHoleName name,
            float widthPaddingPercentage)
        {
            _name = name;
            _widthPaddingPercentage = widthPaddingPercentage;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var edgesInfo = new[] { new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(true, true) };
            var holeRect = GetHoleBounds(outerBounds);

            if (_name == DigitHoleName.Top)
            {
                yield return new DigitChiselResult(holeRect.ToPathPoints(), edgesInfo);
            }
            else if (_name == DigitHoleName.Bottom)
            {
                yield return new DigitChiselResult(holeRect.ToPathPoints(), edgesInfo);
            }
            else
                throw new NotSupportedException();
        }

        private RectangleF GetHoleBounds(RectangleF outerBounds)
        {
            var digitLineWidth = _widthPaddingPercentage * outerBounds.Width;

            PointF topLeft;
            if (_name == DigitHoleName.Top)
            {
                topLeft = new PointF(outerBounds.Left + digitLineWidth, outerBounds.Top + digitLineWidth);
            }
            else if (_name == DigitHoleName.Bottom)
            {
                topLeft = new PointF(outerBounds.Left + digitLineWidth, outerBounds.Top + outerBounds.Height / 2 + digitLineWidth / 2);
            }
            else
                throw new NotSupportedException();

            return new RectangleF(topLeft, new SizeF(outerBounds.Width - 2 * digitLineWidth, (outerBounds.Height - 3 * digitLineWidth) / 2));
        }
    }

    public enum DigitHoleBevel
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft
    }

    public enum DigitHoleName
    {
        Top,
        Bottom
    }
}
