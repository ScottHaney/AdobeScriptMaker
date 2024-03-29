﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions
{
    public class DigitVerticalBar : IDigitChisleAction
    {
        private readonly DigitVerticalBarName _name;
        private readonly float _widthPaddingPercentage;

        public bool OffsetHeightForDigit5 { get; set; }

        public float OverhangPercentage { get; set; }
        public float? FixedOverhangHeight { get; set; }

        public DigitVerticalBar(DigitVerticalBarName name,
            float widthPaddingPercentage)
        {
            _name = name;
            _widthPaddingPercentage = widthPaddingPercentage;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var dimension = _widthPaddingPercentage * outerBounds.Width;

            if (_name == DigitVerticalBarName.TopLeft)
            {
                var topLeft = new PointF(0, FixedOverhangHeight == null ? dimension : (dimension + FixedOverhangHeight.Value));
                var bottomRight = new PointF(dimension, outerBounds.Height / 2 - dimension / 2);

                var rect = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));

                var actualOverhangHeight = FixedOverhangHeight == null ? (OverhangPercentage * rect.Height) : 0;
                rect = new RectangleF(new PointF(rect.Left, rect.Top + actualOverhangHeight), new SizeF(rect.Size.Width, rect.Size.Height - actualOverhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                yield return new DigitChiselResult(rect, new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, false));
            }
            else if (_name == DigitVerticalBarName.TopRight)
            {
                var heightOffset = OffsetHeightForDigit5 ? dimension / 2 : 0;

                var topLeft = new PointF(outerBounds.Width - dimension, dimension);
                var bottomRight = new PointF(outerBounds.Width, outerBounds.Height / 2 - dimension / 2);

                var originalRectHeight = bottomRight.Y - topLeft.Y;
                var rect = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, originalRectHeight - heightOffset));

                var overhangHeight = OverhangPercentage * originalRectHeight;
                rect = new RectangleF(new PointF(rect.Left, rect.Top + overhangHeight), new SizeF(rect.Size.Width, rect.Size.Height - overhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                yield return new DigitChiselResult(rect, new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(true, true));
            }
            else if (_name == DigitVerticalBarName.BottomRight)
            {
                var topLeft = new PointF(outerBounds.Width - dimension, outerBounds.Height / 2 + dimension / 2);
                var bottomRight = new PointF(outerBounds.Width, outerBounds.Height - dimension);

                var rect = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));

                var overhangHeight = OverhangPercentage * rect.Height;
                rect = new RectangleF(rect.TopLeft(), new SizeF(rect.Size.Width, rect.Size.Height - overhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                yield return new DigitChiselResult(rect, new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, false), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(true, true));
            }
            else if (_name == DigitVerticalBarName.BottomLeft)
            {
                var heightOffset = OffsetHeightForDigit5 ? dimension / 2 : 0;

                var topLeft = new PointF(0, outerBounds.Height / 2 + dimension / 2);
                var bottomRight = new PointF(dimension, outerBounds.Height - dimension);

                var originalRectHeight = bottomRight.Y - topLeft.Y;
                var rect = new RectangleF(new PointF(topLeft.X, topLeft.Y - heightOffset), new SizeF(bottomRight.X - topLeft.X, originalRectHeight + heightOffset));

                var overhangHeight = OverhangPercentage * originalRectHeight;
                rect = new RectangleF(rect.TopLeft(), new SizeF(rect.Size.Width, rect.Size.Height - overhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                yield return new DigitChiselResult(rect, new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, false));
            }
            else
                throw new NotSupportedException();
        }
    }

    public enum DigitVerticalBarName
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
