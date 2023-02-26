using Geometry;
using Geometry.Lines;
using Geometry.LineSegments;
using IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How
{
    public class NavyDigitsRenderer
    {
        private readonly SizeF _boundingBoxSize;

        public NavyDigitsRenderer(SizeF boundingBoxSize)
        {
            _boundingBoxSize = boundingBoxSize;
        }

        public string CreateScript()
        {
            var digitsPerRow = 5;
            var xGapPerDigit = 100;
            var yGapPerDigit = 150;

            var result = new StringBuilder();

            for (int i = 0; i <= 9; i++)
            {
                var rowIndex = (i / digitsPerRow);
                var columnIndex = i % digitsPerRow;

                var xOffset = (columnIndex + 1) * xGapPerDigit + columnIndex * _boundingBoxSize.Width;
                var yOffset = (rowIndex + 1) * yGapPerDigit + rowIndex * _boundingBoxSize.Height;
                var topLeft = new PointF(xOffset, yOffset);

                result.AppendLine(CreateDigitScript(i, new RectangleF(topLeft, _boundingBoxSize)));
            }

            return result.ToString();
        }

        private string CreateDigitScript(int digit, RectangleF boundingBox)
        {
            var widthPaddingPercentage = 0.2f;
            var strokeWidth = 1;

            if (digit == 0)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, 45, DigitHoleBevelName.TopLeft, DigitHoleBevelName.TopRight),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, 45, DigitHoleBevelName.BottomRight, DigitHoleBevelName.BottomLeft),
                    new DigitCrossBar(widthPaddingPercentage))
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

                return sculpture.Carve();
            }
            else if (digit == 1)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitOneChisler(widthPaddingPercentage))
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

                return sculpture.Carve();
            }
            else if (digit == 2)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, 45, DigitHoleBevelName.TopLeft),
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, 45, DigitHoleBevelName.TopLeft, DigitHoleBevelName.TopRight, DigitHoleBevelName.BottomRight),
                    new DigitVerticalBar(DigitVerticalBarName.BottomRight, widthPaddingPercentage),
                    new DigitVerticalBar(DigitVerticalBarName.TopLeft, widthPaddingPercentage) { OverhangPercentage = 0.3f },
                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45) { MoveToCenter = true },
                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45) { MoveToCenter = true })
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

                return sculpture.Carve();
            }
            else if (digit == 3)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, 45, DigitHoleBevelName.TopLeft, DigitHoleBevelName.TopRight, DigitHoleBevelName.BottomRight),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, 45, DigitHoleBevelName.TopRight, DigitHoleBevelName.BottomRight, DigitHoleBevelName.BottomLeft),
                    new DigitCrossBar(widthPaddingPercentage) { ExtendLeft = true, RightPadding = 0.5f },
                    new DigitVerticalBar(DigitVerticalBarName.TopLeft, widthPaddingPercentage) { OverhangPercentage = 0.3f },
                    new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = 0.3f },
                    new DigitTriangleInset(DigitTriangleInsetName.Right, 0.25f * widthPaddingPercentage, 45))
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

                return sculpture.Carve();
            }
            else if (digit == 4)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitFourChisler(widthPaddingPercentage, widthPaddingPercentage, 0.65f, 0.65f, 0.25f))
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

                return sculpture.Carve();
            }
            else if (digit == 5)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, 45),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, 45, DigitHoleBevelName.TopRight, DigitHoleBevelName.BottomRight, DigitHoleBevelName.BottomLeft),
                    new DigitVerticalBar(DigitVerticalBarName.TopRight, widthPaddingPercentage),
                    new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = 0.3f },
                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45) { MoveToCenter = true })
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

                return sculpture.Carve();
            }
            else if (digit == 6)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, 45, DigitHoleBevelName.TopLeft, DigitHoleBevelName.TopRight),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, 45, DigitHoleBevelName.All),
                    new DigitVerticalBar(DigitVerticalBarName.TopRight, widthPaddingPercentage) { OverhangPercentage = 0.3f },
                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45) { MoveToCenter = true })
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

                return sculpture.Carve();
            }
            else if (digit == 7)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitSevenChisler(0.85f, widthPaddingPercentage))
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

                return sculpture.Carve();
            }
            else if (digit == 8)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, 45, DigitHoleBevelName.All),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, 45, DigitHoleBevelName.All),
                    new DigitTriangleInset(DigitTriangleInsetName.Left, 0.25f * widthPaddingPercentage, 45),
                    new DigitTriangleInset(DigitTriangleInsetName.Right, 0.25f * widthPaddingPercentage, 45))
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

                return sculpture.Carve();
            }
            else if (digit == 9)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, 45, DigitHoleBevelName.All),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, 45, DigitHoleBevelName.BottomRight, DigitHoleBevelName.BottomLeft),
                    new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = 0.3f},
                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45) { MoveToCenter = true })
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

                return sculpture.Carve();
            }
            else
                return string.Empty;
        }
    }
}
