using Geometry;
using Geometry.Lines;
using Geometry.LineSegments;
using IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public string CreateScript(params int[] indicesToInclude)
        {
            indicesToInclude = indicesToInclude ?? Array.Empty<int>();

            var digitsPerRow = 5;
            var xGapPerDigit = 100;
            var yGapPerDigit = 150;

            var result = new StringBuilder();

            for (int i = 0; i <= 9; i++)
            {
                if (!indicesToInclude.Contains(i))
                    continue;

                var rowIndex = (i / digitsPerRow);
                var columnIndex = i % digitsPerRow;

                var xOffset = (columnIndex + 1) * xGapPerDigit + columnIndex * _boundingBoxSize.Width;
                var yOffset = (rowIndex + 1) * yGapPerDigit + rowIndex * _boundingBoxSize.Height;
                var topLeft = new PointF(xOffset, yOffset);

                result.AppendLine(CreateDigitScript(i, new RectangleF(topLeft, _boundingBoxSize), i.ToString()));
            }

            return result.ToString();
        }

        private string CreateDigitScript(int digit, RectangleF boundingBox, string id)
        {
            var widthPaddingPercentage = 0.25f;
            var triangleInsetPaddingPercentage = 0.5f * widthPaddingPercentage;
            var holeWidthPaddingPercentage = 0.20f;
            var overhangPercentage = 0.4f;
            var shadowWidthPercentage = 1 / 8.0f;

            var strokeWidth = 0;

            var factory = new DigitSculptureFactory(widthPaddingPercentage,
                holeWidthPaddingPercentage,
                overhangPercentage,
                shadowWidthPercentage,
                triangleInsetPaddingPercentage,
                strokeWidth);

            var sculpture = factory.Create(boundingBox, digit);
            sculpture.Id = id;

            return sculpture.Carve();
        }

        public class DigitSculptureFactory
        {
            private readonly float widthPaddingPercentage;
            private readonly float holeWidthPaddingPercentage;
            private readonly float overhangPercentage;
            private readonly float shadowWidthPercentage;
            private readonly float triangleInsetPaddingPercentage;
            private readonly int strokeWidth;

            public DigitSculptureFactory(float _widthPaddingPercentage, float _holeWidthPaddingPercentage, float _overhangPercentage, float _shadowWidthPercentage, float _triangleInsetPaddingPercentage, int _strokeWidth)
            {
                widthPaddingPercentage = _widthPaddingPercentage;
                holeWidthPaddingPercentage = _holeWidthPaddingPercentage;
                overhangPercentage = _overhangPercentage;
                shadowWidthPercentage = _shadowWidthPercentage;
                triangleInsetPaddingPercentage = _triangleInsetPaddingPercentage;
                strokeWidth = _strokeWidth;
            }

            public DigitSculpture Create(RectangleF boundingBox, int digit)
            {
                if (digit == 0)
                    return CreateZero(boundingBox);
                else if (digit == 1)
                    return CreateOne(boundingBox);
                else if (digit == 2)
                    return CreateTwo(boundingBox);
                else if (digit == 3)
                    return CreateThree(boundingBox);
                else if (digit == 4)
                    return CreateFive(boundingBox);
                else if (digit == 5)
                    return CreateFive(boundingBox);
                else if (digit == 6)
                    return CreateSix(boundingBox);
                else if (digit == 7)
                    return CreateSeven(boundingBox);
                else if (digit == 8)
                    return CreateEight(boundingBox);
                else if (digit == 9)
                    return CreateNine(boundingBox);
                else
                    throw new NotSupportedException();
            }

            private DigitSculpture CreateZero(RectangleF boundingBox )
            {
                var sculpture = new DigitSculpture(boundingBox,
                        new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                        new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                        new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                        new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                        new DigitHole(DigitHoleName.Top, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.TopLeft, DigitHoleBevelName.TopRight),
                        new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.BottomRight, DigitHoleBevelName.BottomLeft),
                        new DigitCrossBar(widthPaddingPercentage))
                { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

                return sculpture;
            }

            private DigitSculpture CreateOne(RectangleF boundingBox)
            {
                var sculpture = new DigitSculpture(boundingBox,
                                    new DigitOneChisler(widthPaddingPercentage))
                { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

                return sculpture;
            }

            private DigitSculpture CreateTwo(RectangleF boundingBox)
            {
                var topHole = new DigitHole(DigitHoleName.Top, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.TopLeft, DigitHoleBevelName.TopRight, DigitHoleBevelName.BottomRight);
                var topHolePoints = topHole.GetPoints(boundingBox);

                var firstTwoPoints = topHolePoints.First().Points.Take(2).ToArray();
                var height = (float)Math.Abs(firstTwoPoints[0].Y - firstTwoPoints[1].Y);

                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.TopLeft),
                    topHole,
                    new DigitVerticalBar(DigitVerticalBarName.BottomRight, widthPaddingPercentage),
                    new DigitVerticalBar(DigitVerticalBarName.TopLeft, widthPaddingPercentage) { OverhangPercentage = overhangPercentage, FixedOverhangHeight = height },
                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45) { MoveToCenter = true },
                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45) { MoveToCenter = true })
                { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

                return sculpture;
            }

            private DigitSculpture CreateThree(RectangleF boundingBox)
            {
                var sculpture = new DigitSculpture(boundingBox,
                                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.TopLeft, DigitHoleBevelName.TopRight, DigitHoleBevelName.BottomRight),
                                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.TopRight, DigitHoleBevelName.BottomRight, DigitHoleBevelName.BottomLeft),
                                    new DigitCrossBar(widthPaddingPercentage) { ExtendLeft = true, RightPadding = 0.55f },
                                    new DigitVerticalBar(DigitVerticalBarName.TopLeft, widthPaddingPercentage) { OverhangPercentage = overhangPercentage },
                                    new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = overhangPercentage },
                                    new DigitTriangleInset(DigitTriangleInsetName.Right, triangleInsetPaddingPercentage, 45))
                { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

                return sculpture;
            }

            private DigitSculpture CreateFour(RectangleF boundingBox)
            {
                var sculpture = new DigitSculpture(boundingBox,
                                    new DigitFourChisler(widthPaddingPercentage, widthPaddingPercentage, 0.65f, 0.55f, widthPaddingPercentage))
                { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

                return sculpture;
            }

            private DigitSculpture CreateFive(RectangleF boundingBox)
            {
                var sculpture = new DigitSculpture(boundingBox,
                                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45) { OffsetHeightForDigit5 = true },
                                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.TopRight, DigitHoleBevelName.BottomRight, DigitHoleBevelName.BottomLeft) { OffsetHeightForDigit5 = true },
                                    new DigitVerticalBar(DigitVerticalBarName.TopRight, widthPaddingPercentage) { OffsetHeightForDigit5 = true },
                                    new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = overhangPercentage, OffsetHeightForDigit5 = true },
                                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45) { MoveToCenter = true, OffsetHeightForDigit5 = true })
                { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

                return sculpture;
            }

            private DigitSculpture CreateSix(RectangleF boundingBox)
            {
                var sculpture = new DigitSculpture(boundingBox,
                                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.TopLeft, DigitHoleBevelName.TopRight),
                                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.All),
                                    new DigitVerticalBar(DigitVerticalBarName.TopRight, widthPaddingPercentage) { OverhangPercentage = overhangPercentage },
                                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45) { MoveToCenter = true })
                { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

                return sculpture;
            }

            private DigitSculpture CreateSeven(RectangleF boundingBox)
            {
                var sculpture = new DigitSculpture(boundingBox,
                                    new DigitSevenChisler(0.85f, widthPaddingPercentage))
                { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

                return sculpture;
            }

            private DigitSculpture CreateEight(RectangleF boundingBox)
            {
                var sculpture = new DigitSculpture(boundingBox,
                                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.All),
                                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.All),
                                    new DigitTriangleInset(DigitTriangleInsetName.Left, triangleInsetPaddingPercentage, 45),
                                    new DigitTriangleInset(DigitTriangleInsetName.Right, triangleInsetPaddingPercentage, 45))
                { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

                return sculpture;
            }

            private DigitSculpture CreateNine(RectangleF boundingBox)
            {
                var sculpture = new DigitSculpture(boundingBox,
                                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.All),
                                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.BottomRight, DigitHoleBevelName.BottomLeft),
                                    new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = overhangPercentage },
                                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45) { MoveToCenter = true })
                { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

                return sculpture;
            }
        }
    }
}
