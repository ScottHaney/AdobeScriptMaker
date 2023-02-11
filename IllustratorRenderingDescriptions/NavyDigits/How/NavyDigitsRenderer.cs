﻿using AdobeComponents.Animation;
using AdobeComponents.Components;
using IllustratorRenderingDescriptions.NavyDigits.What;
using RenderingDescriptions.How;
using RenderingDescriptions.Timing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How
{
    public class NavyDigitsRenderer
    {
        private readonly NavyDigitsRenderingDescription _description;

        public NavyDigitsRenderer(NavyDigitsRenderingDescription description)
        {
            _description = description;
        }

        public string CreateScript()
        {
            var sculpture = new DigitSculpture(new RectangleF(0, 0, 500, 800));

            throw new NotImplementedException();

            /*float sizeRatio = 1.2f;
            float horizontalLinesPadding = 0.1f;
            float verticalLinesPadding = 0.1f;

            var width = _description.Dimension;
            var height = width * sizeRatio;

            float horizontalPaddingPercentage = 0.2f;
            float verticalPaddingPercentage = 0.2f;

            float cornerTrianglePercentage = 0.2f;
            float 

            var digitBoundingBox = new RectangleF(new PointF(300, 300), new SizeF(width, height));
            var rectanglePositioner = new RectanglePositioner(digitBoundingBox);

            var pointsCreator = new PointsCreator(new PointF(digitBoundingBox.Left, digitBoundingBox.Top));

            pointsCreator.MoveToX(rectanglePositioner.GetInteriorLeftX(horizontalLinesPadding));
            pointsCreator.MoveToX(rectanglePositioner.GetInteriorRightX(horizontalLinesPadding));

            pointsCreator.Move(rectanglePositioner.GetInteriorRightX(0),
                rectanglePositioner.GetInteriorTopY(cornerTrianglePercentage));



            var topLeft = new PointF(300, 300);
            var points = new List<PointF>();

            points.Add(new PointF(topLeft.X + _description.Dimension * horizontalPaddingPercentage,
                topLeft.Y));

            points.Add(*/

            /*var xAxis = new AdobePathComponent(new StaticValue<Point>);
            var yAxis = new AdobePathComponent(yAxisValues);

            return new RenderedComponents(new IAdobeLayerComponent[] { new AdobePathGroupComponent(xAxis), new AdobePathGroupComponent(yAxis) }
                .Select(x => new TimedAdobeLayerComponent(x, timing.WhenToStart.Time, timing.WhenToStart.Time + timing.RenderDuration.Time))
                .ToArray());*/
        }
    }


    public interface IDigitCreator
    {
        string Carve();
    }

    public class DigitSculpture : IDigitCreator
    {
        private readonly RectangleF _marble;
        private readonly IDigitChisleAction[] _chiselActions;

        public DigitSculpture(RectangleF marble,
            params IDigitChisleAction[] chiselActions)
        {
            _marble = marble;
            _chiselActions = chiselActions ?? Array.Empty<IDigitChisleAction>();
        }

        public string Carve()
        {
            var result = new List<PointF[]>();
            foreach (var chiselAction in _chiselActions)
            {
                result.Add(chiselAction.GetPoints(_marble));
            }

            return ConvertToScript(result);
        }

        private string ConvertToScript(List<PointF[]> chiseledOutSections)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDigitChisleAction
    {
        PointF[] GetPoints(RectangleF outerBounds);
    }

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

        public PointF[] GetPoints(RectangleF outerBounds)
        {
            var digitLineWidth = _widthPaddingPercentage * outerBounds.Width;

            if (_name == DigitHoleName.Top)
            {
                var upperRect = new RectangleF(outerBounds.TopLeft(), new SizeF(outerBounds.Width, outerBounds.Height / 2));

                return new[]
                {
                    new PointF(outerBounds.Left + digitLineWidth, outerBounds.Top + digitLineWidth),
                    new PointF(outerBounds.Right - digitLineWidth, outerBounds.Top + digitLineWidth),
                    new PointF(outerBounds.Right - digitLineWidth, upperRect.Bottom - digitLineWidth / 2),
                    new PointF(outerBounds.Left + digitLineWidth, upperRect.Bottom - digitLineWidth / 2)
                };
            }
            else if (_name == DigitHoleName.Bottom)
            {
                var lowerRect = new RectangleF(new PointF(outerBounds.Left, outerBounds.Top + outerBounds.Height / 2), new SizeF(outerBounds.Width, outerBounds.Height / 2));

                return new[]
                {
                    new PointF(outerBounds.Left + digitLineWidth, lowerRect.Top + digitLineWidth / 2),
                    new PointF(outerBounds.Right - digitLineWidth, lowerRect.Top + digitLineWidth / 2),
                    new PointF(outerBounds.Right - digitLineWidth, lowerRect.Bottom - digitLineWidth),
                    new PointF(outerBounds.Left + digitLineWidth, lowerRect.Bottom - digitLineWidth)
                };
            }
            else
                throw new NotSupportedException();
        }
    }

    public enum DigitHoleName
    {
        Top,
        Bottom
    }

    public class DigitTriangleInset : IDigitChisleAction
    {
        private readonly DigitTriangleInsetName _name;
        private readonly float _widthPercentage;
        private readonly float _angle;

        public DigitTriangleInset(DigitTriangleInsetName name,
            float widthPercentage,
            float angle)
        {
            _name = name;
            _widthPercentage = widthPercentage;
            _angle = angle;
        }

        public PointF[] GetPoints(RectangleF outerBounds)
        {
            var insetLength = _widthPercentage * outerBounds.Width;
            var slope = Math.Tan(_angle * (Math.PI / 180));

            var sidesYOffset = (float)(slope * insetLength);
            var midpointY = outerBounds.Top + outerBounds.Height / 2;

            if (_name == DigitTriangleInsetName.Left)
            {
                return new[]
                {
                    new PointF(outerBounds.Left, midpointY + sidesYOffset),
                    new PointF(outerBounds.Left + insetLength, midpointY),
                    new PointF(outerBounds.Left, midpointY - sidesYOffset)
                };
            }
            else if (_name == DigitTriangleInsetName.Right)
            {
                return new[]
                {
                    new PointF(outerBounds.Right, midpointY - sidesYOffset),
                    new PointF(outerBounds.Right - insetLength, midpointY),
                    new PointF(outerBounds.Right, midpointY + sidesYOffset)
                };
            }
            else
                throw new NotSupportedException();
        }
    }

    public enum DigitTriangleInsetName
    {
        Left,
        Right
    }
    
    public class DigitCrossBar : IDigitChisleAction
    {
        private readonly float _lineWidthPercentage;

        public DigitCrossBar(float lineWidthPercentage)
        {
            _lineWidthPercentage = lineWidthPercentage;
        }

        public PointF[] GetPoints(RectangleF outerBounds)
        {
            var lineHeight = _lineWidthPercentage * outerBounds.Width;
            var middleY = outerBounds.Top + outerBounds.Height / 2;

            return new[]
            {
                new PointF(outerBounds.Left + lineHeight, middleY - lineHeight / 2),
                new PointF(outerBounds.Right - lineHeight, middleY - lineHeight / 2),
                new PointF(outerBounds.Right - lineHeight, middleY + lineHeight / 2),
                new PointF(outerBounds.Left + lineHeight, middleY + lineHeight / 2)
            };
        }
    }

    public enum DigitCornerName
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft
    }

    public class DigitCorner : IDigitChisleAction
    {
        private readonly DigitCornerName _cornerName;
        private readonly float _widthPercentage;
        private readonly float _angle;

        public bool MoveToCenter { get; set; }

        public DigitCorner(DigitCornerName cornerName,
            float widthPercentage,
            float angle)
        {
            _cornerName = cornerName;
            _widthPercentage = widthPercentage;
            _angle = angle;
        }

        public PointF[] GetPoints(RectangleF outerBounds)
        {
            var points = GetPointsInternal(_cornerName, outerBounds);

            if (MoveToCenter)
            {
                var centerBarHeight = _widthPercentage * outerBounds.Width;
                var distance = outerBounds.Height / 2 - centerBarHeight / 2;

                var cornerPoint = points.Skip(1).First();
                var shift = cornerPoint.Y == outerBounds.Top
                    ? distance
                    : -distance;

                return points
                    .Select(x => new PointF(x.X, x.Y + shift))
                    .ToArray();
            }
            else
                return points;
        }

        private PointF[] GetPointsInternal(DigitCornerName cornerName, RectangleF outerBounds)
        {
            var xLength = outerBounds.Width * _widthPercentage;
            var slope = (float)Math.Tan(_angle * (Math.PI / 180));

            if (cornerName == DigitCornerName.TopLeft)
            {
                var topLeft = outerBounds.TopLeft();

                var refPoint = new PointF(topLeft.X + xLength, topLeft.Y);
                var intersectionPoint = new PointF(topLeft.X, topLeft.Y + xLength * slope);


                return new PointF[]
                {
                    intersectionPoint,
                    topLeft,
                    refPoint
                };
            }
            else if (cornerName == DigitCornerName.TopRight)
            {
                var topRight = outerBounds.TopRight();

                var refPoint = new PointF(topRight.X - xLength, topRight.Y);
                var intersectionPoint = new PointF(topRight.X, topRight.Y + xLength * slope);


                return new PointF[]
                {
                    refPoint,
                    topRight,
                    intersectionPoint
                };
            }
            else if (cornerName == DigitCornerName.BottomRight)
            {
                var bottomRight = outerBounds.BottomRight();

                var refPoint = new PointF(bottomRight.X - xLength, bottomRight.Y);
                var intersectionPoint = new PointF(bottomRight.X, bottomRight.Y - xLength * slope);

                return new PointF[]
                {
                    intersectionPoint,
                    bottomRight,
                    refPoint
                };
            }
            else if (cornerName == DigitCornerName.BottomLeft)
            {
                var bottomLeft = outerBounds.BottomLeft();

                var refPoint = new PointF(bottomLeft.X + xLength, bottomLeft.Y);
                var intersectionPoint = new PointF(bottomLeft.X, bottomLeft.Y - xLength * slope);

                return new PointF[]
                {
                    refPoint,
                    bottomLeft,
                    intersectionPoint
                };
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }

    public static class DigitCornerNameExtensions
    {
        public static bool IsOnRight(this DigitCornerName cornerName)
        {
            return cornerName == DigitCornerName.TopRight && cornerName == DigitCornerName.BottomRight;
        }

        public static bool IsOnLeft(this DigitCornerName cornerName)
        {
            return cornerName == DigitCornerName.TopLeft && cornerName == DigitCornerName.BottomLeft;
        }
    }

    public static class RectangleFExtensions
    {
        public static PointF TopLeft(this RectangleF rect)
        {
            return new PointF(rect.Left, rect.Top);
        }

        public static PointF TopRight(this RectangleF rect)
        {
            return new PointF(rect.Right, rect.Top);
        }

        public static PointF BottomRight(this RectangleF rect)
        {
            return new PointF(rect.Right, rect.Bottom);
        }

        public static PointF BottomLeft(this RectangleF rect)
        {
            return new PointF(rect.Left, rect.Bottom);
        }
    }

    public class RectanglePositioner
    {
        private readonly RectangleF _bounds;

        public RectanglePositioner(RectangleF bounds)
        {
            _bounds = bounds;
        }

        public float GetInteriorLeftX(float padding)
        {
            return _bounds.Left + padding * _bounds.Width;
        }

        public float GetInteriorRightX(float padding)
        {
            return _bounds.Right - padding * _bounds.Width;
        }

        public float GetInteriorTopY(float padding)
        {
            return _bounds.Top + padding * _bounds.Height;
        }

        public float GetInteriorBottomY(float padding)
        {
            return _bounds.Bottom - padding * _bounds.Height;
        }
    }

    public class PointsCreator
    {
        private readonly List<PointF> _points = new List<PointF>();

        private PointF _currentPoint => _points.Any() ? _points.Last() : _startPoint;
        private readonly PointF _startPoint;

        public PointsCreator(PointF startPoint)
        {
            _startPoint = startPoint;
        }

        public void MoveToX(float xValue)
        {
            _points.Add(new PointF(xValue, _currentPoint.Y));
        }

        public void MoveToY(float yValue)
        {
            _points.Add(new PointF(_currentPoint.X, yValue));
        }

        public void Move(float xValue, float yValue)
        {
            _points.Add(new PointF(xValue, yValue));
        }
    }

    public class LinesCreator
    {
        private readonly PointsCreator _pointsCreator;

        public LinesCreator(PointsCreator pointsCreator)
        {
            _pointsCreator = pointsCreator;
        }

        public void AddHorizontalLine(float xValue, float length)
        {

        }
    }
}
