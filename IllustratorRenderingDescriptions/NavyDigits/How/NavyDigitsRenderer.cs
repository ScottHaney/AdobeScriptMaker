﻿using Geometry;
using Geometry.Lines;
using Geometry.LineSegments;
using IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How
{
    public class NavyDigitsRenderer
    {
        private readonly SizeF _boundingBoxSize;
        private readonly DigitSculptureFactory _sculptureFactory;

        public NavyDigitsRenderer(SizeF boundingBoxSize, DigitSculptureFactory sculptureFactory)
        {
            _boundingBoxSize = boundingBoxSize;
            _sculptureFactory= sculptureFactory;
        }

        public string CreateSingleDigitScript(params int[] indicesToInclude)
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

        public string CreateNumberScript(params int[] digits)
        {
            var xGapPerDigit = _boundingBoxSize.Width * 0.30f;

            var result = new StringBuilder();

            for (int i = 0; i < digits.Length; i++)
            {
                var xOffset = 150 + i * xGapPerDigit + i * _boundingBoxSize.Width;
                var yOffset = 150;
                var topLeft = new PointF(xOffset, yOffset);

                result.AppendLine(CreateDigitScript(digits[i], new RectangleF(topLeft, _boundingBoxSize), Guid.NewGuid().ToString("N")));
            }

            return result.ToString();
        }

        private string CreateDigitScript(int digit, RectangleF boundingBox, string id)
        {
            var sculpture = _sculptureFactory.Create(boundingBox, digit);
            sculpture.Id = id;

            return sculpture.Carve();
        }
    }

    public class DigitSculptureFactory
    {
        private readonly float widthPaddingPercentage;
        private readonly float holeWidthPaddingPercentage;
        private readonly float overhangPercentage;
        private readonly float shadowWidthPercentage;
        private readonly float triangleInsetPaddingPercentage;
        private readonly int strokeWidth;
        private readonly float cornerWidthPaddingPercentage;

        public DigitHoleWidthPaddingProvider CustomDigitHoleWidthPaddingProviderForSix { get; set; }

        public DigitSculptureFactory(float _widthPaddingPercentage, float _holeWidthPaddingPercentage, float _overhangPercentage, float _shadowWidthPercentage, float _triangleInsetPaddingPercentage, int _strokeWidth, float? _cornerWidthPaddingPercentage = null)
        {
            widthPaddingPercentage = _widthPaddingPercentage;
            holeWidthPaddingPercentage = _holeWidthPaddingPercentage;
            overhangPercentage = _overhangPercentage;
            shadowWidthPercentage = _shadowWidthPercentage;
            triangleInsetPaddingPercentage = _triangleInsetPaddingPercentage;
            strokeWidth = _strokeWidth;
            cornerWidthPaddingPercentage = _cornerWidthPaddingPercentage ?? _widthPaddingPercentage;
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
                return CreateFour(boundingBox);
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

        private DigitSculpture CreateZero(RectangleF boundingBox)
        {
            var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, cornerWidthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.TopRight, cornerWidthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomRight, cornerWidthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, cornerWidthPaddingPercentage, 45),
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
                new DigitCorner(DigitCornerName.TopLeft, cornerWidthPaddingPercentage, 45),
                new DigitCorner(DigitCornerName.TopRight, cornerWidthPaddingPercentage, 45),
                new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.TopLeft),
                topHole,
                new DigitVerticalBar(DigitVerticalBarName.BottomRight, widthPaddingPercentage),
                new DigitVerticalBar(DigitVerticalBarName.TopLeft, widthPaddingPercentage) { OverhangPercentage = overhangPercentage, FixedOverhangHeight = height },
                new DigitCorner(DigitCornerName.TopLeft, cornerWidthPaddingPercentage, 45) { MoveToCenter = true },
                new DigitCorner(DigitCornerName.BottomRight, cornerWidthPaddingPercentage, 45) { MoveToCenter = true })
            { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

            return sculpture;
        }

        private DigitSculpture CreateThree(RectangleF boundingBox)
        {
            var sculpture = new DigitSculpture(boundingBox,
                                new DigitCorner(DigitCornerName.TopLeft, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.TopRight, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.BottomLeft, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.BottomRight, cornerWidthPaddingPercentage, 45),
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
                                new DigitCorner(DigitCornerName.BottomLeft, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.BottomRight, cornerWidthPaddingPercentage, 45),
                                new DigitHole(DigitHoleName.Top, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45) { OffsetHeightForDigit5 = true },
                                new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.TopRight, DigitHoleBevelName.BottomRight, DigitHoleBevelName.BottomLeft) { OffsetHeightForDigit5 = true },
                                new DigitVerticalBar(DigitVerticalBarName.TopRight, widthPaddingPercentage) { OffsetHeightForDigit5 = true },
                                new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = overhangPercentage, OffsetHeightForDigit5 = true },
                                new DigitCorner(DigitCornerName.TopRight, cornerWidthPaddingPercentage, 45) { MoveToCenter = true, OffsetHeightForDigit5 = true })
            { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

            return sculpture;
        }

        private DigitSculpture CreateSix(RectangleF boundingBox)
        {
            var isCv6 = CustomDigitHoleWidthPaddingProviderForSix is CV6DigitHoleWidthPaddingProvider;
            DigitHoleBevelName[] bevelNames;
            if (isCv6)
                bevelNames = new[] { DigitHoleBevelName.TopLeft, DigitHoleBevelName.TopRight, DigitHoleBevelName.BottomLeft };
            else
                bevelNames = new[] { DigitHoleBevelName.TopLeft, DigitHoleBevelName.TopRight };

            var sculpture = new DigitSculpture(boundingBox,
                                new DigitCorner(DigitCornerName.TopLeft, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.TopRight, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.BottomLeft, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.BottomRight, cornerWidthPaddingPercentage, 45),
                                new DigitHole(DigitHoleName.Top, widthPaddingPercentage, CustomDigitHoleWidthPaddingProviderForSix ?? new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, bevelNames),
                                new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.All),
                                new DigitVerticalBar(DigitVerticalBarName.TopRight, widthPaddingPercentage) { OverhangPercentage = overhangPercentage },
                                new DigitCorner(DigitCornerName.TopRight, cornerWidthPaddingPercentage, 45) { MoveToCenter = true })
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
                                new DigitCorner(DigitCornerName.TopLeft, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.TopRight, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.BottomLeft, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.BottomRight, cornerWidthPaddingPercentage, 45),
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
                                new DigitCorner(DigitCornerName.TopLeft, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.TopRight, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.BottomLeft, cornerWidthPaddingPercentage, 45),
                                new DigitCorner(DigitCornerName.BottomRight, cornerWidthPaddingPercentage, 45),
                                new DigitHole(DigitHoleName.Top, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.All),
                                new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(holeWidthPaddingPercentage), 45, DigitHoleBevelName.BottomRight, DigitHoleBevelName.BottomLeft),
                                new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = overhangPercentage },
                                new DigitCorner(DigitCornerName.BottomLeft, cornerWidthPaddingPercentage, 45) { MoveToCenter = true })
            { StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

            return sculpture;
        }
    }
}
