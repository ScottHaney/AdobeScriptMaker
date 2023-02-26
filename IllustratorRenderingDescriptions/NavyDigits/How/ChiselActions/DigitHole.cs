﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions
{
    public class DigitHole : IDigitChisleAction
    {
        private readonly DigitHoleName _name;
        private readonly float _widthPaddingPercentage;
        private readonly float _angle;

        private readonly DigitHoleBevelName[] _bevelNames;

        public DigitHole(DigitHoleName name,
            float widthPaddingPercentage,
            float angle,
            params DigitHoleBevelName[] bevels)
        {
            _name = name;
            _widthPaddingPercentage = widthPaddingPercentage;
            _angle = angle;
            _bevelNames = bevels;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var edgesInfo = new[] { new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(true, true) };
            var holeRect = GetHoleBounds(outerBounds);

            if (_name == DigitHoleName.Top)
            {
                yield return AddInBevels(holeRect, holeRect.ToPathPoints(), edgesInfo);
            }
            else if (_name == DigitHoleName.Bottom)
            {
                yield return AddInBevels(holeRect, holeRect.ToPathPoints(), edgesInfo);
            }
            else
                throw new NotSupportedException();
        }

        private DigitChiselResult AddInBevels(RectangleF bounds, PointF[] points, ChiselEdgeInfo[] edgesInfo)
        {
            if (!_bevelNames.Any())
                return new DigitChiselResult(points, edgesInfo);

            var pointsResult = new List<PointF>();
            var edgesResult = new List<ChiselEdgeInfo>();
            for (int i = 0; i < points.Length; i++)
            {
                var bevelValue = (DigitHoleBevelName)i;
                if (_bevelNames.Contains(bevelValue) || _bevelNames.Contains(DigitHoleBevelName.All))
                {
                    var cornerCreator = new DigitCornerPointsCreator((DigitCornerName)i, _widthPaddingPercentage, _angle);
                    var cornerResult = cornerCreator.Create(bounds);

                    pointsResult.Add(cornerResult.HypotenusePoints.First());

                    var orientation = (bevelValue == DigitHoleBevelName.TopLeft || bevelValue == DigitHoleBevelName.BottomLeft)
                        ? MarbleOrientations.Negative
                        : MarbleOrientations.Positive;

                    edgesResult.Add(new ChiselEdgeInfo(bevelValue == DigitHoleBevelName.TopLeft, orientation));

                    edgesResult.Add(edgesInfo[i]);
                    pointsResult.Add(cornerResult.HypotenusePoints.Last());
                }
                else
                {
                    pointsResult.Add(points[i]);
                    edgesResult.Add(edgesInfo[i]);
                }
            }

            return new DigitChiselResult(pointsResult.ToArray(), edgesResult.ToArray());
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

    public enum DigitHoleBevelName
    {
        TopLeft = 0,
        TopRight = 1,
        BottomRight = 2,
        BottomLeft = 3,
        All = 4
    }

    public enum DigitHoleName
    {
        Top,
        Bottom
    }
}
