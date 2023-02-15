using AdobeComponents.Animation;
using AdobeComponents.Components;
using IllustratorRenderingDescriptions.NavyDigits.What;
using RenderingDescriptions.How;
using RenderingDescriptions.Timing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

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
                currentTopLeft = new PointF(currentTopLeft.X + _boundingBoxSize.Width + 100, currentTopLeft.Y);
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
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitOneChisler(0.2f))
                { Id = digit.ToString() };

                return sculpture.Carve();
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
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitFourChisler(0.2f, 0.2f, 0.65f, 0.65f, 0.25f))
                { Id = digit.ToString() };

                return sculpture.Carve();
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
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitSevenChisler(0.85f, 0.25f))
                { Id = digit.ToString() };

                return sculpture.Carve();
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

    public interface IDigitScriptCreator
    {
        string Create(IEnumerable<PointF[]> chiseledOutPieces);
    }


    public interface IDigitCreator
    {
        string Carve();
    }

    public class DigitOneChisler : IDigitChisleAction
    {
        private readonly float _lineWidthPercentage;

        public DigitOneChisler(float lineWidthPercentage)
        {
            _lineWidthPercentage = lineWidthPercentage;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var lineWidth = _lineWidthPercentage * outerBounds.Width;

            var leftOverSpace = outerBounds.Width - lineWidth;
            var leftCutout = new RectangleF(outerBounds.TopLeft(), new SizeF(leftOverSpace / 2, outerBounds.Height));

            yield return new DigitChiselResult(leftCutout, RectangleSideName.Left);

            var rightCutout = new RectangleF(outerBounds.Right - leftOverSpace / 2, outerBounds.Top, leftOverSpace / 2, outerBounds.Height);
            yield return new DigitChiselResult(rightCutout);
        }
    }

    public class DigitFourChisler : IDigitChisleAction
    {
        private readonly float _verticalLineWidthPercentage;
        private readonly float _horizontalLineHeightPercentage;
        private readonly float _linesIntersectionXPercentage;
        private readonly float _linesIntersectionYPercentage;
        private readonly float _slantBarWidthPercentage;

        public DigitFourChisler(float verticalLineWidthPercentage,
            float horizontalLineHeightPercentage,
            float linesIntersectionXPercentage,
            float linesIntersectionYPercentage,
            float slantBarWidthPercentage)
        {
            _verticalLineWidthPercentage = verticalLineWidthPercentage;
            _horizontalLineHeightPercentage = horizontalLineHeightPercentage;
            _linesIntersectionXPercentage = linesIntersectionXPercentage;
            _linesIntersectionYPercentage = linesIntersectionYPercentage;
            _slantBarWidthPercentage = slantBarWidthPercentage;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF marble)
        {
            var verticalLineWidth = _verticalLineWidthPercentage * marble.Width;
            var horizontalLineHeight = _horizontalLineHeightPercentage * marble.Width;
            var slantBarWidth = _slantBarWidthPercentage * marble.Width;

            var intersectionX = marble.Left + _linesIntersectionXPercentage * marble.Width;
            var intersectionY = marble.Top + _linesIntersectionYPercentage * marble.Height;

            var innerRect = new RectangleF(
                intersectionX - verticalLineWidth / 2,
                intersectionY + horizontalLineHeight / 2,
                verticalLineWidth,
                horizontalLineHeight);

            var bottomLeftRect = new RectangleF(marble.Left, innerRect.Bottom, innerRect.Left - marble.Left, marble.Bottom - innerRect.Bottom);
            yield return new DigitChiselResult(bottomLeftRect, RectangleSideName.Top);

            var bottomRightRect = new RectangleF(innerRect.Right, innerRect.Bottom, marble.Right - innerRect.Right, marble.Bottom - innerRect.Bottom);
            yield return new DigitChiselResult(bottomRightRect, RectangleSideName.Left, RectangleSideName.Top);

            var topRightRect = new RectangleF(innerRect.Right, marble.Top, marble.Right - innerRect.Right, innerRect.Top - marble.Top);
            yield return new DigitChiselResult(topRightRect, RectangleSideName.Left);

            var topLeftRect = new RectangleF(marble.Left, marble.Top, innerRect.Left - marble.Left, innerRect.Top - marble.Top);
            var topLeftTriangle = new PointF[] { topLeftRect.BottomLeft(), topLeftRect.TopLeft(), topLeftRect.TopRight() };
            yield return new DigitChiselResult(topLeftTriangle, false, false, false);

            var slope = (topLeftRect.Bottom - topLeftRect.Top) / (topLeftRect.Right - topLeftRect.Left);
            var topLeftCutoutBottomLeftPoint = new PointF(topLeftRect.Left + slantBarWidth, topLeftRect.Bottom);
            var topLeftCutoutBottomRightPoint = innerRect.TopLeft();

            var topLeftCutoutTopPoint = new PointF(topLeftCutoutBottomRightPoint.X, topLeftCutoutBottomRightPoint.Y - slope * (topLeftRect.Width - slantBarWidth));

            var topLeftCutout = new[]
            {
                topLeftCutoutBottomLeftPoint,
                topLeftCutoutTopPoint,
                topLeftCutoutBottomRightPoint
            };

            yield return new DigitChiselResult(topLeftCutout, true, false, false);
        }
    }

    public class DigitSevenChisler : IDigitChisleAction
    {
        private readonly float _bottomBoxPercentage;
        private readonly float _slantLineWidthPercentage;

        public DigitSevenChisler(float bottomBoxPercentage,
            float slantLineWidthPercentage)
        {
            _bottomBoxPercentage = bottomBoxPercentage;
            _slantLineWidthPercentage = slantLineWidthPercentage;
        }

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var bottomBoxHeight = _bottomBoxPercentage * outerBounds.Height;
            var bottomBox = new RectangleF(outerBounds.Left, outerBounds.Bottom - bottomBoxHeight, outerBounds.Width, bottomBoxHeight);

            var slantLineWidth = _slantLineWidthPercentage * outerBounds.Width;

            var rightSlantLineBottomPoint = new PointF(bottomBox.Left + slantLineWidth, bottomBox.Bottom);
            var rightSlantLineTopPoint = bottomBox.TopRight();

            var leftSlantLineBottomPoint = bottomBox.BottomLeft();
            var leftSlantLineTopPoint = new PointF(bottomBox.Right - slantLineWidth, bottomBox.Top);

            var bottomLeftCutout = new[]
            {
                bottomBox.TopLeft(),
                leftSlantLineTopPoint,
                leftSlantLineBottomPoint
            };
            yield return new DigitChiselResult(bottomLeftCutout, true, false, false);

            var bottomRightCutout = new[]
            {
                rightSlantLineBottomPoint,
                rightSlantLineTopPoint,
                bottomBox.BottomRight()
            };
            yield return new DigitChiselResult(bottomRightCutout, true, false, false);

        }
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
            var result = new List<DigitChiselResult>();
            foreach (var chiselAction in _chiselActions)
            {
                result.AddRange(chiselAction.GetPoints(_marble));
            }

            return ConvertToScript(result);
        }

        private string ConvertToScript(List<DigitChiselResult> chiseledOutSections)
        {
            var script = new StringBuilder();
            script.AppendLine(@"var doc = app.activeDocument;");

            var idPostfix = $"{(string.IsNullOrEmpty(Id) ? "" : $"_{Id}")}";

            //Render the paths
            script.AppendLine(CreatePath(_marble.ToPathPoints(), "doc.pathItems", $"marble{idPostfix}"));

            for (int i = 0; i < chiseledOutSections.Count; i++)
            {
                script.AppendLine(CreatePath(chiseledOutSections[i].Points, "doc.pathItems", $"chiselSection{i}{idPostfix}"));
            }

            script.AppendLine(CreatePathFinderScript("Live Pathfinder Exclude"));
            //This code was taken from: https://community.adobe.com/t5/illustrator-discussions/looking-for-javascript-commands-for-path-finder-operation/m-p/12355960
            /*script.AppendLine(@"app.executeMenuCommand(""group"");
app.executeMenuCommand(""Live Pathfinder Exclude"");
app.executeMenuCommand(""expandStyle"");
app.executeMenuCommand(""ungroup"");
app.activeDocument.selection = null;");

            script.AppendLine($"doc.pathItems[0].name = '{Id}'");*/

            //script.Append(CreateShadowScript(_marble, 0.15f, idPostfix, chiseledOutSections));

            return script.ToString();
        }

        private string CreatePathFinderScript(string pathFinderAction)
        {
            var script = new StringBuilder();

            var pathsCountVar = $"pathsCount_{Guid.NewGuid().ToString("N")}";
            var compoundPathsCountVar = $"compoundPathsCount_{Guid.NewGuid().ToString("N")}";

            script.AppendLine($@"var {pathsCountVar} = doc.pathItems.length;
var {compoundPathsCountVar} = doc.compoundPathItems.length;");

            //This code was taken from: https://community.adobe.com/t5/illustrator-discussions/looking-for-javascript-commands-for-path-finder-operation/m-p/12355960
            script.AppendLine($@"app.executeMenuCommand(""group"");
app.executeMenuCommand(""{pathFinderAction}"");
app.executeMenuCommand(""expandStyle"");
app.executeMenuCommand(""ungroup"");
app.activeDocument.selection = null;");

            script.AppendLine($@"if (doc.compoundPathItems.length == {compoundPathsCountVar} + 1) {{ doc.compoundPathItems[0].name = '{Id}'; }}
else {{ doc.pathItems[0].name = '{Id}'; }}");

            return script.ToString();
        }

        private string CreateShadowScript(RectangleF marble,
            float dimensionPercentage,
            string idPostfix,
            List<DigitChiselResult> chiseledOutSections)
        {
            var script = new StringBuilder();

            var marbleShadowsInfo = new DigitChiselResult(marble, RectangleSideName.Right, RectangleSideName.Bottom);
            var allLineInfos = chiseledOutSections
                .Concat(new[] { marbleShadowsInfo })
                .SelectMany(x => x.ShadowsInfo.ShadowLineInfos)
                .ToList();

            var shadowLines = allLineInfos.Where(x => x.CastsShadow).ToList();
            var removeShadowLines = allLineInfos.Where(x => !x.CastsShadow).ToList();


            var shadowLineIndex = 0;
            foreach (var shadowLine in shadowLines)
            {
                script.AppendLine(CreatePath(new[] { shadowLine.Start, shadowLine.End }, "doc.pathItems", $"shadow_line{shadowLineIndex}_{idPostfix}", false, true));
                shadowLineIndex++;
            }

            //This code was taken from: https://community.adobe.com/t5/illustrator-discussions/looking-for-javascript-commands-for-path-finder-operation/m-p/12355960
            script.AppendLine(@"app.executeMenuCommand(""group"");
app.executeMenuCommand(""Live Pathfinder Add"");
app.executeMenuCommand(""expandStyle"");
app.executeMenuCommand(""ungroup"");
app.activeDocument.selection = null;");

            var shadowLinesGroupName = $"{Id}_shadow_lines";
            script.AppendLine($"doc.compoundPathItems[doc.compoundPathItems.length - 1].name = '{shadowLinesGroupName}'");

            var removeShadowLineIndex = 0;
            foreach (var removeShadowLine in removeShadowLines)
            {
                script.AppendLine(CreatePath(new[] { removeShadowLine.Start, removeShadowLine.End }, "doc.pathItems", $"remove_shadow_line{removeShadowLineIndex}_{idPostfix}", false, true));
                removeShadowLineIndex++;
            }

            //This code was taken from: https://community.adobe.com/t5/illustrator-discussions/looking-for-javascript-commands-for-path-finder-operation/m-p/12355960
            script.AppendLine(@"app.executeMenuCommand(""group"");
app.executeMenuCommand(""Live Pathfinder Add"");
app.executeMenuCommand(""expandStyle"");
app.executeMenuCommand(""ungroup"");
app.activeDocument.selection = null;");

            var removeShadowLinesGroupName = $"{Id}_remove_shadow_lines";
            script.AppendLine($"doc.compoundPathItems[doc.compoundPathItems.length - 1].name = '{removeShadowLinesGroupName}'");

            script.AppendLine($"doc.compoundPathItems.findByName('{shadowLinesGroupName}').selected = true;");
            script.AppendLine($"doc.compoundPathItems.findByName('{removeShadowLinesGroupName}').selected = true;");

            //This code was taken from: https://community.adobe.com/t5/illustrator-discussions/looking-for-javascript-commands-for-path-finder-operation/m-p/12355960
            script.AppendLine(@"app.executeMenuCommand(""group"");
app.executeMenuCommand(""Live Pathfinder Exclude"");
app.executeMenuCommand(""expandStyle"");
app.executeMenuCommand(""ungroup"");
app.activeDocument.selection = null;");

            script.AppendLine($"doc.compoundPathItems[doc.compoundPathItems.length - 1].name = '{Id}_shadows'");

            return script.ToString();
        }

        private string CreatePath(PointF[] points, string pathItems, string variableName, bool isClosed = true, bool isBlack = false)
        {
            return $@"var {variableName} = {pathItems}.add();
{variableName}.setEntirePath({CreateJavaScriptArray(points)});
{variableName}.closed = true;
{variableName}.selected = true;
{variableName}.fillColor = new RGBColor();
{variableName}.fillColor.red = {(isBlack ? 0 : 255)};
{variableName}.fillColor.green = 0;
{variableName}.fillColor.blue = 0;
{variableName}.name = '{variableName}'";
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
        IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds);
    }

    public class DigitChiselResult
    {
        public readonly PointF[] Points;
        public readonly ChiselShadowsInfo ShadowsInfo;

        public DigitChiselResult(PointF[] points,
            params bool[] isShadowSide)
        {
            Points = points;

            var items = new List<ShadowLineInfo>();
            var sideIndex = 0;
            foreach (var pair in points.Zip(points.Skip(1), (x, y) => new { Start = x, End = y }))
            {
                items.Add(new ShadowLineInfo(pair.Start, pair.End, isShadowSide[sideIndex]));
                sideIndex++;
            }

            ShadowsInfo = new ChiselShadowsInfo(items);
        }

        public DigitChiselResult(PointF[] points,
            ChiselShadowsInfo shadowsInfo)
        {
            Points = points;
            ShadowsInfo = shadowsInfo;
        }

        public DigitChiselResult(RectangleF rect,
            params RectangleSideName[] shadowSides)
        {
            Points = rect.ToPathPoints();

            ShadowsInfo = new ChiselShadowsInfo(
                GetSides(rect)
                    .Select(x => new ShadowLineInfo(x.Start, x.End, shadowSides.Contains(x.Name)))
                    .ToList());
        }

        private IEnumerable<RectangleSide> GetSides(RectangleF rect)
        {
            yield return new RectangleSide(rect.BottomLeft(), rect.TopLeft(), RectangleSideName.Left);
            yield return new RectangleSide(rect.TopLeft(), rect.TopRight(), RectangleSideName.Top);
            yield return new RectangleSide(rect.TopRight(), rect.BottomRight(), RectangleSideName.Right);
            yield return new RectangleSide(rect.BottomRight(), rect.BottomLeft(), RectangleSideName.Bottom);
        }

        private class RectangleSide
        {
            public readonly PointF Start;
            public readonly PointF End;
            public readonly RectangleSideName Name;

            public RectangleSide(PointF start,
                PointF end,
                RectangleSideName name)
            {
                Start = start;
                End = end;
                Name = name;
            }
        }
    }

    public enum RectangleSideName
    {
        Left,
        Top,
        Right,
        Bottom
    }

    public class ChiselShadowsInfo
    {
        public readonly List<ShadowLineInfo> ShadowLineInfos;

        public static readonly ChiselShadowsInfo Empty = new ChiselShadowsInfo(new List<ShadowLineInfo>());

        public ChiselShadowsInfo(List<ShadowLineInfo> shadowLineInfos)
        {
            ShadowLineInfos = shadowLineInfos;
        }
    }

    public class ShadowLineInfo
    {
        public readonly PointF Start;
        public readonly PointF End;

        public readonly bool CastsShadow;

        public ShadowLineInfo(PointF start,
            PointF end,
            bool castsShadow)
        {
            Start = start;
            End = end;
            CastsShadow = castsShadow;
        }
    }

    public enum DigitChiselLocation
    {
        Left,
        Center,
        Right
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

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
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
                yield return new DigitChiselResult(rect, RectangleSideName.Top);
            }
            else if (_name == DigitVerticalBarName.TopRight)
            {
                var topLeft = new PointF(outerBounds.Width - dimension, dimension);
                var bottomRight = new PointF(outerBounds.Width, outerBounds.Height / 2 - dimension / 2);

                var rect = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));

                var overhangHeight = OverhangPercentage * rect.Height;
                rect = new RectangleF(new PointF(rect.Left, rect.Top + overhangHeight), new SizeF(rect.Size.Width, rect.Size.Height - overhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                yield return new DigitChiselResult(rect, RectangleSideName.Top, RectangleSideName.Left);
            }
            else if (_name == DigitVerticalBarName.BottomRight)
            {
                var topLeft = new PointF(outerBounds.Width - dimension, outerBounds.Height / 2 + dimension / 2);
                var bottomRight = new PointF(outerBounds.Width, outerBounds.Height - dimension);

                var rect = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));

                var overhangHeight = OverhangPercentage * rect.Height;
                rect = new RectangleF(rect.TopLeft(), new SizeF(rect.Size.Width, rect.Size.Height - overhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                yield return new DigitChiselResult(rect, RectangleSideName.Top, RectangleSideName.Left);
            }
            else if (_name == DigitVerticalBarName.BottomLeft)
            {
                var topLeft = new PointF(0, outerBounds.Height / 2 + dimension / 2);
                var bottomRight = new PointF(dimension, outerBounds.Height - dimension);

                var rect = new RectangleF(topLeft, new SizeF(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));

                var overhangHeight = OverhangPercentage * rect.Height;
                rect = new RectangleF(rect.TopLeft(), new SizeF(rect.Size.Width, rect.Size.Height - overhangHeight));

                rect.Location = new PointF(outerBounds.TopLeft().X + rect.TopLeft().X, outerBounds.TopLeft().Y + rect.TopLeft().Y);
                yield return new DigitChiselResult(rect, RectangleSideName.Top);
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

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var digitLineWidth = _widthPaddingPercentage * outerBounds.Width;

            if (_name == DigitHoleName.Top)
            {
                var upperRect = new RectangleF(outerBounds.TopLeft(), new SizeF(outerBounds.Width, outerBounds.Height / 2));

                yield return new DigitChiselResult(new[]
                {
                    new PointF(outerBounds.Left + digitLineWidth, outerBounds.Top + digitLineWidth),
                    new PointF(outerBounds.Right - digitLineWidth, outerBounds.Top + digitLineWidth),
                    new PointF(outerBounds.Right - digitLineWidth, upperRect.Bottom - digitLineWidth / 2),
                    new PointF(outerBounds.Left + digitLineWidth, upperRect.Bottom - digitLineWidth / 2)
                }, true, false, false, true);
            }
            else if (_name == DigitHoleName.Bottom)
            {
                var lowerRect = new RectangleF(new PointF(outerBounds.Left, outerBounds.Top + outerBounds.Height / 2), new SizeF(outerBounds.Width, outerBounds.Height / 2));

                yield return new DigitChiselResult(new[]
                {
                    new PointF(outerBounds.Left + digitLineWidth, lowerRect.Top + digitLineWidth / 2),
                    new PointF(outerBounds.Right - digitLineWidth, lowerRect.Top + digitLineWidth / 2),
                    new PointF(outerBounds.Right - digitLineWidth, lowerRect.Bottom - digitLineWidth),
                    new PointF(outerBounds.Left + digitLineWidth, lowerRect.Bottom - digitLineWidth)
                }, true, false, false, true);
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

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var insetLength = _widthPercentage * outerBounds.Width;
            var slope = Math.Tan(_angle * (Math.PI / 180));

            var sidesYOffset = (float)(slope * insetLength);
            var midpointY = outerBounds.Top + outerBounds.Height / 2;

            if (_name == DigitTriangleInsetName.Left)
            {
                yield return new DigitChiselResult(new[]
                {
                    new PointF(outerBounds.Left, midpointY + sidesYOffset),
                    new PointF(outerBounds.Left + insetLength, midpointY),
                    new PointF(outerBounds.Left, midpointY - sidesYOffset)
                }, false, true, false);
            }
            else if (_name == DigitTriangleInsetName.Right)
            {
                yield return new DigitChiselResult(new[]
                {
                    new PointF(outerBounds.Right, midpointY - sidesYOffset),
                    new PointF(outerBounds.Right - insetLength, midpointY),
                    new PointF(outerBounds.Right, midpointY + sidesYOffset)
                }, true, false, false);
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

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var lineHeight = _lineWidthPercentage * outerBounds.Width;
            var middleY = outerBounds.Top + outerBounds.Height / 2;

            var crossBarWidth = (outerBounds.Right - outerBounds.Left) - 2 * lineHeight;

            var leftShift = ExtendLeft ? lineHeight : 0;
            var rightShift = RightPadding * crossBarWidth;

            yield return new DigitChiselResult(new[]
            {
                new PointF(outerBounds.Left + lineHeight - leftShift, middleY - lineHeight / 2),
                new PointF(outerBounds.Right - lineHeight - rightShift, middleY - lineHeight / 2),
                new PointF(outerBounds.Right - lineHeight - rightShift, middleY + lineHeight / 2),
                new PointF(outerBounds.Left + lineHeight - leftShift, middleY + lineHeight / 2)
            }, true, false, false, true);
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

        public IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds)
        {
            var result = GetResultInternal(_cornerName, outerBounds);

            if (MoveToCenter)
            {
                var centerBarHeight = _widthPercentage * outerBounds.Width;
                var distance = outerBounds.Height / 2 - centerBarHeight / 2;

                var cornerPoint = result.Points.Skip(1).First();
                var shift = cornerPoint.Y == outerBounds.Top
                    ? distance
                    : -distance;

                yield return new DigitChiselResult(result.Points
                    .Select(x => new PointF(x.X, x.Y + shift))
                    .ToArray(), result.ShadowsInfo);
            }
            else
                yield return result;
        }

        private DigitChiselResult GetResultInternal(DigitCornerName cornerName, RectangleF outerBounds)
        {
            var xLength = outerBounds.Width * _widthPercentage;
            var slope = (float)Math.Tan(_angle * (Math.PI / 180));

            if (cornerName == DigitCornerName.TopLeft)
            {
                var topLeft = outerBounds.TopLeft();

                var refPoint = new PointF(topLeft.X + xLength, topLeft.Y);
                var intersectionPoint = new PointF(topLeft.X, topLeft.Y + xLength * slope);

                var shadowSides = MoveToCenter
                    ? new[] { false, true, false }
                    : new[] { false, false, false };

                return new DigitChiselResult(new PointF[]
                {
                    intersectionPoint,
                    topLeft,
                    refPoint
                }, shadowSides);
            }
            else if (cornerName == DigitCornerName.TopRight)
            {
                var topRight = outerBounds.TopRight();

                var refPoint = new PointF(topRight.X - xLength, topRight.Y);
                var intersectionPoint = new PointF(topRight.X, topRight.Y + xLength * slope);

                var shadowSides = MoveToCenter
                    ? new[] { true, false, false }
                    : new[] { false, false, true };

                return new DigitChiselResult(new PointF[]
                {
                    refPoint,
                    topRight,
                    intersectionPoint
                }, shadowSides);
            }
            else if (cornerName == DigitCornerName.BottomRight)
            {
                var bottomRight = outerBounds.BottomRight();

                var refPoint = new PointF(bottomRight.X - xLength, bottomRight.Y);
                var intersectionPoint = new PointF(bottomRight.X, bottomRight.Y - xLength * slope);

                var shadowSides = MoveToCenter
                    ? new[] { false, false, true }
                    : new[] { false, false, true };

                return new DigitChiselResult(new PointF[]
                {
                    intersectionPoint,
                    bottomRight,
                    refPoint
                }, shadowSides);
            }
            else if (cornerName == DigitCornerName.BottomLeft)
            {
                var bottomLeft = outerBounds.BottomLeft();

                var refPoint = new PointF(bottomLeft.X + xLength, bottomLeft.Y);
                var intersectionPoint = new PointF(bottomLeft.X, bottomLeft.Y - xLength * slope);

                var shadowSides = MoveToCenter
                    ? new[] { false, false, true }
                    : new[] { false, false, false };

                return new DigitChiselResult(new PointF[]
                {
                    refPoint,
                    bottomLeft,
                    intersectionPoint
                }, shadowSides);
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
