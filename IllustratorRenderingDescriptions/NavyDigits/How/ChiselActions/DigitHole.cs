using System;
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
        private readonly DigitHoleWidthPaddingProvider _innerWidthPaddingProvider;
        private readonly float _angle;

        private readonly DigitHoleBevelName[] _bevelNames;

        public bool OffsetHeightForDigit5 { get; set; }

        public DigitHole(DigitHoleName name,
            float widthPaddingPercentage,
            DigitHoleWidthPaddingProvider innerWidthPaddingProvider,
            float angle,
            params DigitHoleBevelName[] bevels)
        {
            _name = name;
            _widthPaddingPercentage = widthPaddingPercentage;
            _innerWidthPaddingProvider = innerWidthPaddingProvider;
            _angle = angle;
            _bevelNames = bevels;
        }

        public DigitHole(DigitHoleName name,
            float widthPaddingPercentage,
            float angle,
            params DigitHoleBevelName[] bevels)
            : this(name, widthPaddingPercentage, new ConstantDigitHoleWidthPaddingProvider(widthPaddingPercentage), angle, bevels)
        { }

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
                    var paddingPercentage = _innerWidthPaddingProvider.GetWidthPaddingPercentage(bevelValue);
                    var cornerCreator = new DigitCornerPointsCreator((DigitCornerName)i, paddingPercentage, _angle);
                    var cornerResult = cornerCreator.Create(bounds);

                    if (paddingPercentage < 0)
                    {
                        edgesResult.Add(new ChiselEdgeInfo(false, MarbleOrientations.Positive));
                        pointsResult.Add(cornerResult.AllPoints[0]);

                        edgesResult.Add(edgesInfo[i]);
                        pointsResult.Add(cornerResult.AllPoints[1]);
                    }
                    else
                    {
                        pointsResult.Add(cornerResult.HypotenusePoints.First());

                        var orientation = (bevelValue == DigitHoleBevelName.TopLeft || bevelValue == DigitHoleBevelName.BottomLeft)
                            ? MarbleOrientations.Negative
                            : MarbleOrientations.Positive;

                        edgesResult.Add(new ChiselEdgeInfo(bevelValue == DigitHoleBevelName.TopLeft, orientation));

                        edgesResult.Add(edgesInfo[i]);
                        pointsResult.Add(cornerResult.HypotenusePoints.Last());
                    }
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
            float heightOffset = 0;
            if (_name == DigitHoleName.Top)
            {
                if (OffsetHeightForDigit5)
                {
                    heightOffset = -digitLineWidth / 2;
                }

                topLeft = new PointF(outerBounds.Left + digitLineWidth, outerBounds.Top + digitLineWidth);
            }
            else if (_name == DigitHoleName.Bottom)
            {
                if (OffsetHeightForDigit5)
                {
                    heightOffset = digitLineWidth / 2;
                    topLeft = new PointF(outerBounds.Left + digitLineWidth, outerBounds.Top + outerBounds.Height / 2 + digitLineWidth / 2 - heightOffset);
                }
                else
                    topLeft = new PointF(outerBounds.Left + digitLineWidth, outerBounds.Top + outerBounds.Height / 2 + digitLineWidth / 2);
            }
            else
                throw new NotSupportedException();

            return new RectangleF(topLeft, new SizeF(outerBounds.Width - 2 * digitLineWidth, (outerBounds.Height - 3 * digitLineWidth) / 2 + heightOffset));
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

    public abstract class DigitHoleWidthPaddingProvider
    {
        public abstract float GetWidthPaddingPercentage(DigitHoleBevelName name);
    }

    public class ConstantDigitHoleWidthPaddingProvider : DigitHoleWidthPaddingProvider
    {
        private readonly float _widthPercentage;

        public ConstantDigitHoleWidthPaddingProvider(float widthPercentage)
        {
            _widthPercentage = widthPercentage;
        }

        public override float GetWidthPaddingPercentage(DigitHoleBevelName name)
        {
            return _widthPercentage;
        }
    }

    public class CV6DigitHoleWidthPaddingProvider : DigitHoleWidthPaddingProvider
    {
        private readonly float _widthPercentage;

        public CV6DigitHoleWidthPaddingProvider(float widthPercentage)
        {
            _widthPercentage = widthPercentage;
        }

        public override float GetWidthPaddingPercentage(DigitHoleBevelName name)
        {
            if (name == DigitHoleBevelName.BottomLeft)
                return -_widthPercentage;
            else
                return _widthPercentage;
        }
    }
}
