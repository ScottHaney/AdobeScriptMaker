using AdobeComponents.Animation;
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
    public class NavyDigitsRenderer : IHowToRender
    {
        private readonly NavyDigitsRenderingDescription _description;

        public NavyDigitsRenderer(NavyDigitsRenderingDescription description)
        {
            _description = description;
        }

        public RenderedComponents Render(ITimingForRender timing)
        {
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
        List<PointF> Carve();
    }

    public class DigitSculpture : IDigitCreator
    {
        private readonly RectangleF _marble;
        private readonly IDigitCorner _cornerDescription;

        public DigitSculpture(RectangleF marble,
            IDigitCorner cornerDescription)
        {
            _marble = marble;
            _cornerDescription = cornerDescription;
        }

        public List<PointF> Carve()
        {
            var corners = new[] { DigitCornerName.TopLeft, DigitCornerName.TopRight, DigitCornerName.BottomRight, DigitCornerName.BottomLeft };

            var points = new List<PointF>();
            foreach (var corner in corners)
            {
                var trianglePoints = _cornerDescription.GetPoints(corner, _marble);
                points.Add(trianglePoints.First());
                points.Add(trianglePoints.Last());
            }

            return points;
        }
    }

    public interface IDigitHole
    {
        PointF[] GetPoints(DigitHoleName name, RectangleF outerBounds);
    }

    public class DigitHole : IDigitHole
    {
        private readonly float _widthPaddingPercentage;

        public DigitHole(float widthPaddingPercentage)
        {
            _widthPaddingPercentage = widthPaddingPercentage;
        }

        public PointF[] GetPoints(DigitHoleName name, RectangleF outerBounds)
        {
            var digitLineWidth = _widthPaddingPercentage * outerBounds.Width;

            if (name == DigitHoleName.Top)
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
            else if (name == DigitHoleName.Bottom)
            {
                throw new NotImplementedException();
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

    public interface IDigitCorner
    {
        PointF[] GetPoints(DigitCornerName cornerName, RectangleF outerBounds);
    }

    public enum DigitCornerName
    {
        TopLeft,
        TopRight,
        BottomRight,
        BottomLeft
    }

    public class DigitCorner : IDigitCorner
    {
        private readonly float _widthPercentage;
        private readonly float _angle;

        public DigitCorner(float widthPercentage,
            float angle)
        {
            _widthPercentage = widthPercentage;
            _angle = angle;
        }

        public PointF[] GetPoints(DigitCornerName cornerName, RectangleF outerBounds)
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
