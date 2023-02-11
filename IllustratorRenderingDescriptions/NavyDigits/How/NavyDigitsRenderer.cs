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
    public class NavyDigitsRenderer
    {
        private readonly SizeF _boundingBoxSize;

        public NavyDigitsRenderer(SizeF boundingBoxSize)
        {
            _boundingBoxSize = boundingBoxSize;
        }

        public string CreateScript()
        {
            var result = new StringBuilder();
            var currentTopLeft = new PointF(0, 0);

            for (int i = 0; i <= 9; i++)
            {
                result.AppendLine(CreateDigitScript(i, new RectangleF(currentTopLeft, _boundingBoxSize)));
                currentTopLeft = new PointF(currentTopLeft.X + _boundingBoxSize.Width, currentTopLeft.Y);
            }

            return result.ToString();
        }

        private string CreateDigitScript(int digit, RectangleF boundingBox)
        {
            if (digit == 0)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, 0.1f, 45),
                    new DigitCorner(DigitCornerName.TopRight, 0.1f, 45),
                    new DigitCorner(DigitCornerName.BottomRight, 0.1f, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, 0.1f, 45),
                    new DigitHole(DigitHoleName.Top, 0.2f),
                    new DigitHole(DigitHoleName.Bottom, 0.2f),
                    new DigitCrossBar(0.2f))
                { Id = digit.ToString() };

                return sculpture.Carve();
            }
            else if (digit == 1)
            {
                return string.Empty;
            }
            else if (digit == 2)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, 0.15f, 45),
                    new DigitCorner(DigitCornerName.TopRight, 0.15f, 45),
                    new DigitHole(DigitHoleName.Top, 0.2f),
                    new DigitHole(DigitHoleName.Bottom, 0.2f),
                    new DigitVerticalBar(DigitVerticalBarName.BottomRight, 0.2f),
                    new DigitVerticalBar(DigitVerticalBarName.TopLeft, 0.2f) { OverhangPercentage = 0.3f },
                    new DigitCorner(DigitCornerName.TopLeft, 0.2f, 45) { MoveToCenter = true },
                    new DigitCorner(DigitCornerName.BottomRight, 0.2f, 45) { MoveToCenter = true })
                { Id = digit.ToString() };

                return sculpture.Carve();
            }
            else if (digit == 3)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, 0.15f, 45),
                    new DigitCorner(DigitCornerName.TopRight, 0.15f, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, 0.15f, 45),
                    new DigitCorner(DigitCornerName.BottomRight, 0.15f, 45),
                    new DigitHole(DigitHoleName.Top, 0.2f),
                    new DigitHole(DigitHoleName.Bottom, 0.2f),
                    new DigitCrossBar(0.2f) { ExtendLeft = true, RightPadding = 0.3f },
                    new DigitVerticalBar(DigitVerticalBarName.TopLeft, 0.2f) { OverhangPercentage = 0.3f },
                    new DigitVerticalBar(DigitVerticalBarName.BottomLeft, 0.2f) { OverhangPercentage = 0.3f },
                    new DigitTriangleInset(DigitTriangleInsetName.Right, 0.05f, 45))
                { Id = digit.ToString() };

                return sculpture.Carve();
            }
            else if (digit == 4)
            {
                return string.Empty;
            }
            else if (digit == 5)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.BottomLeft, 0.15f, 45),
                    new DigitCorner(DigitCornerName.BottomRight, 0.15f, 45),
                    new DigitHole(DigitHoleName.Top, 0.2f),
                    new DigitHole(DigitHoleName.Bottom, 0.2f),
                    new DigitVerticalBar(DigitVerticalBarName.TopRight, 0.2f),
                    new DigitVerticalBar(DigitVerticalBarName.BottomLeft, 0.2f) { OverhangPercentage = 0.3f },
                    new DigitCorner(DigitCornerName.TopRight, 0.2f, 45) { MoveToCenter = true })
                { Id = digit.ToString() };

                return sculpture.Carve();
            }
            else if (digit == 6)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, 0.15f, 45),
                    new DigitCorner(DigitCornerName.TopRight, 0.15f, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, 0.15f, 45),
                    new DigitCorner(DigitCornerName.BottomRight, 0.15f, 45),
                    new DigitHole(DigitHoleName.Top, 0.2f),
                    new DigitHole(DigitHoleName.Bottom, 0.2f),
                    new DigitVerticalBar(DigitVerticalBarName.TopRight, 0.2f) { OverhangPercentage = 0.3f },
                    new DigitCorner(DigitCornerName.TopRight, 0.2f, 45) { MoveToCenter = true })
                { Id = digit.ToString() };

                return sculpture.Carve();
            }
            else if (digit == 7)
            {
                return string.Empty;
            }
            else if (digit == 8)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, 0.15f, 45),
                    new DigitCorner(DigitCornerName.TopRight, 0.15f, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, 0.15f, 45),
                    new DigitCorner(DigitCornerName.BottomRight, 0.15f, 45),
                    new DigitHole(DigitHoleName.Top, 0.2f),
                    new DigitHole(DigitHoleName.Bottom, 0.2f),
                    new DigitTriangleInset(DigitTriangleInsetName.Right, 0.05f, 45),
                    new DigitTriangleInset(DigitTriangleInsetName.Left, 0.05f, 45))
                { Id = digit.ToString() };

                return sculpture.Carve();
            }
            else if (digit == 9)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, 0.15f, 45),
                    new DigitCorner(DigitCornerName.TopRight, 0.15f, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, 0.15f, 45),
                    new DigitCorner(DigitCornerName.BottomRight, 0.15f, 45),
                    new DigitHole(DigitHoleName.Top, 0.2f),
                    new DigitHole(DigitHoleName.Bottom, 0.2f),
                    new DigitVerticalBar(DigitVerticalBarName.BottomLeft, 0.2f) { OverhangPercentage = 0.3f},
                    new DigitCorner(DigitCornerName.BottomLeft, 0.2f, 45) { MoveToCenter = true })
                { Id = digit.ToString() };

                return sculpture.Carve();
            }
            else
                return string.Empty;
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

        public string Id { get; set; }

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
            var script = new StringBuilder();
            script.AppendLine(@"var doc = app.activeDocument;");

            var idPostfix = $"{(string.IsNullOrEmpty(Id) ? "" : $"_{Id}")}";

            //Render the paths
            script.AppendLine(CreatePath(_marble.ToPathPoints(), "doc.pathItems", $"marble{idPostfix}"));


            for (int i = 0; i < chiseledOutSections.Count; i++)
            {
                script.AppendLine(CreatePath(chiseledOutSections[i], "doc.pathItems", $"chiselSection{i}{idPostfix}"));
            }

            //This code was taken from: https://community.adobe.com/t5/illustrator-discussions/looking-for-javascript-commands-for-path-finder-operation/m-p/12355960
            script.AppendLine(@"app.executeMenuCommand(""group"");
app.executeMenuCommand(""Live Pathfinder Exclude"");
app.executeMenuCommand(""expandStyle"");
app.executeMenuCommand(""ungroup"");
app.activeDocument.selection = null;");

            return script.ToString();
        }

        private string CreatePath(PointF[] points, string pathItems, string variableName)
        {
            return $@"var {variableName} = {pathItems}.add();
{variableName}.setEntirePath({CreateJavaScriptArray(points)});
{variableName}.closed = true;
{variableName}.selected = true
{variableName}.fillColor = new RGBColor();
{variableName}.fillColor.red = 255;
{variableName}.fillColor.green = 0;
{variableName}.fillColor.blue = 0;";
        }

        private string CreateJavaScriptArray(PointF[] points)
        {
            //Make sure to slip the points vertically since illustrator renders towards
            //the top of the screen as y increases rather than the standard programming
            //way of having increasing y render towards the bottom of the screen
            return $"[{string.Join(",", points.Select(x => $"[{x.X}, {-x.Y}]"))}]";
        }
    }

    public interface IDigitChisleAction
    {
        PointF[] GetPoints(RectangleF outerBounds);
    }

    public class DigitVerticalBar : IDigitChisleAction
    {
        private readonly DigitVerticalBarName _name;
        private readonly float _widthPaddingPercentage;

        public float OverhangPercentage { get; set; }

        public DigitVerticalBar(DigitVerticalBarName name,
            float widthPaddingPercentage)
        {
            _name = name;
            _widthPaddingPercentage = widthPaddingPercentage;
        }

        public PointF[] GetPoints(RectangleF outerBounds)
        {
            var dimension = _widthPaddingPercentage * outerBounds.Width;

            if (_name == DigitVerticalBarName.TopLeft)
            {
                var topLeft = new PointF(0, dimension);
                var bottomRight = new PointF(dimension, outerBounds.Height / 2 - dimension / 2);

                var rect = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));

                var overhangHeight = OverhangPercentage * rect.Height;
                rect = new RectangleF(new PointF(rect.Left, rect.Top + overhangHeight), new SizeF(rect.Size.Width, rect.Size.Height - overhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                return rect.ToPathPoints();
            }
            else if (_name == DigitVerticalBarName.TopRight)
            {
                var topLeft = new PointF(outerBounds.Width - dimension, dimension);
                var bottomRight = new PointF(outerBounds.Width, outerBounds.Height / 2 - dimension / 2);

                var rect = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));

                var overhangHeight = OverhangPercentage * rect.Height;
                rect = new RectangleF(new PointF(rect.Left, rect.Top + overhangHeight), new SizeF(rect.Size.Width, rect.Size.Height - overhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                return rect.ToPathPoints();
            }
            else if (_name == DigitVerticalBarName.BottomRight)
            {
                var topLeft = new PointF(outerBounds.Width - dimension, outerBounds.Height / 2 + dimension / 2);
                var bottomRight = new PointF(outerBounds.Width, outerBounds.Height - dimension);

                var rect = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));

                var overhangHeight = OverhangPercentage * rect.Height;
                rect = new RectangleF(rect.TopLeft(), new SizeF(rect.Size.Width, rect.Size.Height - overhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                return rect.ToPathPoints();
            }
            else if (_name == DigitVerticalBarName.BottomLeft)
            {
                var topLeft = new PointF(0, outerBounds.Height / 2 + dimension / 2);
                var bottomRight = new PointF(dimension, outerBounds.Height - dimension);

                var rect = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));

                var overhangHeight = OverhangPercentage * rect.Height;
                rect = new RectangleF(rect.TopLeft(), new SizeF(rect.Size.Width, rect.Size.Height - overhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                return rect.ToPathPoints();
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

        public bool ExtendLeft { get; set; }
        public float RightPadding { get; set; }

        public DigitCrossBar(float lineWidthPercentage)
        {
            _lineWidthPercentage = lineWidthPercentage;
        }

        public PointF[] GetPoints(RectangleF outerBounds)
        {
            var lineHeight = _lineWidthPercentage * outerBounds.Width;
            var middleY = outerBounds.Top + outerBounds.Height / 2;

            var crossBarWidth = (outerBounds.Right - outerBounds.Left) - 2 * lineHeight;

            var leftShift = ExtendLeft ? lineHeight : 0;
            var rightShift = RightPadding * crossBarWidth;

            return new[]
            {
                new PointF(outerBounds.Left + lineHeight - leftShift, middleY - lineHeight / 2),
                new PointF(outerBounds.Right - lineHeight - rightShift, middleY - lineHeight / 2),
                new PointF(outerBounds.Right - lineHeight - rightShift, middleY + lineHeight / 2),
                new PointF(outerBounds.Left + lineHeight - leftShift, middleY + lineHeight / 2)
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

        public static PointF[] ToPathPoints(this RectangleF rect)
        {
            return new[]
            {
                rect.TopLeft(),
                rect.TopRight(),
                rect.BottomRight(),
                rect.BottomLeft()
            };
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
