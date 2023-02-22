using AdobeComponents.Animation;
using AdobeComponents.Components;
using Geometry;
using IllustratorRenderingDescriptions.NavyDigits.What;
using RenderingDescriptions.How;
using RenderingDescriptions.Timing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml.XPath;

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
            var strokeWidth = 10;

            if (digit == 0)
            {
                var sculpture = new DigitSculpture(boundingBox,
                    new DigitCorner(DigitCornerName.TopLeft, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.TopRight, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomRight, widthPaddingPercentage, 45),
                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45),
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage),
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
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage),
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
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage),
                    new DigitCrossBar(widthPaddingPercentage) { ExtendLeft = true, RightPadding = 0.3f },
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
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage),
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
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage),
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
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage),
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
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage),
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage),
                    new DigitVerticalBar(DigitVerticalBarName.BottomLeft, widthPaddingPercentage) { OverhangPercentage = 0.3f},
                    new DigitCorner(DigitCornerName.BottomLeft, widthPaddingPercentage, 45) { MoveToCenter = true })
                { Id = digit.ToString(), StrokeWidth = strokeWidth };

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

            yield return new DigitChiselResult(leftCutout);

            var rightCutout = new RectangleF(outerBounds.Right - leftOverSpace / 2, outerBounds.Top, leftOverSpace / 2, outerBounds.Height);
            yield return new DigitChiselResult(rightCutout, RectangleSideName.Left);
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

        public float StrokeWidth { get; set; } = 1;

        public string Id { get; set; }

        public int[] DigitColor { get; set; } = new int[] { 255, 255, 255 };
        public int[] ShadowColor { get; set; } = new int[] { 10, 17, 21 };

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
            script.AppendLine(CreatePath(_marble.ToPathPoints(), "doc.pathItems", $"marble{idPostfix}", DigitColor));

            for (int i = 0; i < chiseledOutSections.Count; i++)
            {
                script.AppendLine(CreatePath(chiseledOutSections[i].Points, "doc.pathItems", $"chiselSection{i}_{idPostfix}", DigitColor));
            }

            script.AppendLine(CreatePathFinderScript("Live Pathfinder Exclude", Id));

            var digitVariableName = $"digit_{Id}_path";

            script.AppendLine(SelectNamedItem(Id));
            script.AppendLine($"var {digitVariableName} = doc.selection[0];");
            script.AppendLine("app.activeDocument.selection = null;");

            var strokeColor = new int[] { 0, 0, 0 };

            if (StrokeWidth > 0)
            {
                script.AppendLine($@"if ({digitVariableName}.typename === 'PathItem') {{
{digitVariableName}.strokeWidth = {StrokeWidth};
{digitVariableName}.strokeColor = new RGBColor({strokeColor[0]},{strokeColor[1]},{strokeColor[2]});
}}
else {{
for (var i = 0; i < {digitVariableName}.pathItems.length; i++) {{
{digitVariableName}.pathItems[i].strokeWidth = {StrokeWidth};
{digitVariableName}.pathItems[i].strokeColor = new RGBColor({strokeColor[0]},{strokeColor[1]},{strokeColor[2]});
}}
}}");
            
            }

            script.Append(CreateShadowScript(_marble, digitVariableName, 0.15f, idPostfix, chiseledOutSections));

            return script.ToString();
        }

        private string CreatePathFinderScript(string pathFinderAction, string resultName, bool ungroup = true, bool group = true)
        {
            var script = new StringBuilder();

            var pathsCountVar = $"pathsCount_{Guid.NewGuid().ToString("N")}";
            var compoundPathsCountVar = $"compoundPathsCount_{Guid.NewGuid().ToString("N")}";

            script.AppendLine($@"var {pathsCountVar} = doc.pathItems.length;
var {compoundPathsCountVar} = doc.compoundPathItems.length;");

            //This code was taken from: https://community.adobe.com/t5/illustrator-discussions/looking-for-javascript-commands-for-path-finder-operation/m-p/12355960
            script.AppendLine($@"{(group ? "app.executeMenuCommand('group')" : "")};
app.executeMenuCommand(""{pathFinderAction}"");
app.executeMenuCommand(""expandStyle"");
{(ungroup ? "app.executeMenuCommand('ungroup')" : "")};
app.activeDocument.selection[0].name = '{resultName}';
app.activeDocument.selection = null;");

            return script.ToString();
        }

        private string SelectNamedItem(string name)
        {
            var matchFoundVar = $"matchFound_{Guid.NewGuid().ToString("N")}";
            return $@"var {matchFoundVar} = false;
for (var i = 0; i < doc.compoundPathItems.length; i++) {{
if (doc.compoundPathItems[i].name == '{name}') {{ doc.compoundPathItems[i].selected = true; {matchFoundVar} = true; break; }}
}}
if (!{matchFoundVar}) {{
for (var i = 0; i < doc.pathItems.length; i++) {{
if (doc.pathItems[i].name == '{name}') {{ doc.pathItems[i].selected = true; {matchFoundVar} = true; break; }}
}}
}}
if (!{matchFoundVar}) {{
for (var i = 0; i < doc.groupItems.length; i++) {{
if (doc.groupItems[i].name == '{name}') {{ doc.groupItems[i].selected = true; {matchFoundVar} = true; break; }}
}}
}}";
        }

        private string FindItemRefByName(string name, StringBuilder scriptBuilder)
        {
            var variableName = $"pathMatch_{Guid.NewGuid().ToString("N")}";
            var matchFoundVar = $"matchFound_{Guid.NewGuid().ToString("N")}";
            var result = $@"var {matchFoundVar} = false;
var {variableName};
for (var i = 0; i < doc.compoundPathItems.length; i++) {{
if (doc.compoundPathItems[i].name == '{name}') {{ {variableName} = doc.compoundPathItems[i]; {matchFoundVar} = true; break; }}
}}
if (!{matchFoundVar}) {{
for (var i = 0; i < doc.pathItems.length; i++) {{
if (doc.pathItems[i].name == '{name}') {{ {variableName} = doc.pathItems[i]; {matchFoundVar} = true; break; }}
}}
}}
if (!{matchFoundVar}) {{
for (var i = 0; i < doc.groupItems.length; i++) {{
if (doc.groupItems[i].name == '{name}') {{{variableName} = doc.groupItems[i]; {matchFoundVar} = true; break; }}
}}
}}";

            scriptBuilder.AppendLine(result);
            return variableName;
        }

        private string CreateShadowScript(RectangleF marble,
            string digitVariableName,
            float dimensionPercentage,
            string idPostfix,
            List<DigitChiselResult> chiseledOutSections,
            float shadowAngle = 45)
        {
            var script = new StringBuilder();

            var shadowsCreator = new DigitShadowLinesCreator(new ShadowCreator(dimensionPercentage, shadowAngle)) { StrokeWidth = StrokeWidth };
            var updatedShadowPaths = shadowsCreator.CreateShadowPaths(marble, chiseledOutSections).ToList();

            var shadowPathsName = $"shadows_{idPostfix}";

            for (int i = 0; i < updatedShadowPaths.Count; i++)
            {
                var duplicateVar = $"dup_digit_{Guid.NewGuid().ToString("N")}";
                script.AppendLine($@"var {duplicateVar} = {digitVariableName}.duplicate();
{duplicateVar}.name = '{duplicateVar}'
{duplicateVar}.selected = true;");

                var itemName = $"{shadowPathsName}_{i}";
                script.AppendLine(CreatePath(updatedShadowPaths[i], "doc.pathItems", $"{itemName}_original", ShadowColor));

                //Remove any parts of the shadows that overlap the digit
                script.AppendLine(CreatePathFinderScript("Live Pathfinder Minus Back", itemName));
                //script.AppendLine("app.executeMenuCommand(\"Live Pathfinder Exclude\");");
                //script.AppendLine("app.activeDocument.selection = null;");

                //Make sure to get rid of the strok that gets added after running the path finder operation
                var updatedShadowRef = FindItemRefByName(itemName, script);
                script.AppendLine($@"{updatedShadowRef}.strokeWidth = 0;
{updatedShadowRef}.strokeColor = new NoColor();");
            }

            //script.AppendLine(CreateCompoundPath(updatedShadowPaths, "doc.compoundPathItems", shadowPathsName, x => $"shadow_line{x}_{idPostfix}", isBlack: true));

            

            //var removeShadowPaths = removeShadowLines
            //    .Select(removeShadowLine => new[] { removeShadowLine.Start, removeShadowLine.End, new PointF(removeShadowLine.End.X + offsetX, removeShadowLine.End.Y + offsetY), new PointF(removeShadowLine.Start.X + offsetX, removeShadowLine.Start.Y + offsetY) });

            //var removeShadowPathsName = $"remove_shadows_{idPostfix}";
            //script.AppendLine(CreateCompoundPath(removeShadowPaths, "doc.compoundPathItems", removeShadowPathsName, x => $"remove_shadow_line{x}_{idPostfix}", isBlue: true));

            //var removeShadowLinesGroupName = $"{Id}_remove_shadow_lines";
            //script.AppendLine(CreatePathFinderScript("Live Pathfinder Add", removeShadowLinesGroupName, false));

            //var shadowPaths = shadowLines
            //    .Select(shadowLine => new[] { shadowLine.Start, shadowLine.End, new PointF(shadowLine.End.X + offsetX, shadowLine.End.Y + offsetY), new PointF(shadowLine.Start.X + offsetX, shadowLine.Start.Y + offsetY) });

            //var shadowPathsName = $"shadows_{idPostfix}";
            //script.AppendLine(CreateCompoundPath(shadowPaths, "doc.compoundPathItems", shadowPathsName, x => $"shadow_line{x}_{idPostfix}", isBlack: true));

            //var shadowLinesGroupName = $"{Id}_shadow_lines";
            //script.AppendLine(CreatePathFinderScript("Live Pathfinder Add", shadowLinesGroupName, false));

            //script.AppendLine(SelectNamedItem(shadowPathsName));
            //script.AppendLine(SelectNamedItem(removeShadowPathsName));

            //script.AppendLine(CreatePathFinderScript("Live Pathfinder Minus Back", ""));
            //script.AppendLine("app.executeMenuCommand(\"Live Pathfinder Minus Back\");");
            script.AppendLine("app.activeDocument.selection = null;");

            return script.ToString();
        }

        private string CreatePath(PointF[] points, string pathItems, string variableName, int[] color, bool isClosed = true)
        {
            return $@"var {variableName} = {pathItems}.add();
{variableName}.setEntirePath({CreateJavaScriptArray(points)});
{variableName}.closed = {isClosed.ToString().ToLower()};
{variableName}.selected = true;
{variableName}.fillColor = new RGBColor();
{variableName}.fillColor.red = {color[0]};
{variableName}.fillColor.green = {color[1]};
{variableName}.fillColor.blue = {color[2]};
{variableName}.name = '{variableName}'";
        }

        private string CreateCompoundPath(IEnumerable<PointF[]> paths, string compoundPathItems, string compoundPathName, Func<int, string> pathNameFunc, bool isClosed = true, bool isBlack = false, bool isBlue = false)
        {
            var red = isBlack ? 0 : (isBlue ? 0 : 255);
            var green = 0;
            var blue = isBlack ? 0 : (isBlue ? 255 : 0);

            var script = new StringBuilder();

            var compoundPathVar = $"compoundPathItem_{Guid.NewGuid().ToString("N")}";
            script.AppendLine($@"var {compoundPathVar} = {compoundPathItems}.add();
{compoundPathVar}.name = '{compoundPathName}';");

            var index = 1;
            foreach (var path in paths)
            {
                var pathItemVar = $"pathItem_{Guid.NewGuid().ToString("N")}";
                script.AppendLine($@"var {pathItemVar} = {compoundPathVar}.pathItems.add()
{pathItemVar}.setEntirePath({CreateJavaScriptArray(path)});
{pathItemVar}.closed = {isClosed.ToString().ToLower()};
{pathItemVar}.fillColor = new RGBColor();
{pathItemVar}.fillColor.red = {red};
{pathItemVar}.fillColor.green = {green};
{pathItemVar}.fillColor.blue = {blue};
{pathItemVar}.name = '{pathNameFunc(index)}'");

                index++;
            }

            script.AppendLine($"{compoundPathVar}.selected = true;");

            return script.ToString();
        }

        private string CreateJavaScriptArray(PointF[] points)
        {
            //Make sure to slip the points vertically since illustrator renders towards
            //the top of the screen as y increases rather than the standard programming
            //way of having increasing y render towards the bottom of the screen
            return $"[{string.Join(",", points.Select(x => $"[{x.X}, {-x.Y}]"))}]";
        }
    }

    public class ShadowCreator
    {
        private readonly float _dimensionPercentage;
        private readonly float _shadowAngle;

        public ShadowCreator(float dimensionPercentage, float shadowAngle)
        {
            _dimensionPercentage = dimensionPercentage;
            _shadowAngle = shadowAngle;
        }

        public PointF[] CreateShadowPath(Line shadowLine, RectangleF marble)
        {
            double shadowDimension = _dimensionPercentage * marble.Width;

            var offsetX = shadowDimension * Math.Cos(_shadowAngle * (Math.PI / 180));
            var offsetY = shadowDimension * Math.Sin(_shadowAngle * (Math.PI / 180));


            return new PointF[] { shadowLine.Start, shadowLine.End, new PointF((float)(shadowLine.End.X + offsetX), (float)(shadowLine.End.Y + offsetY)), new PointF((float)(shadowLine.Start.X + offsetX), (float)(shadowLine.Start.Y + offsetY)) };
        }

        public PointF[] CreateAntiShadowPath(Line removeShadowLine, RectangleF marble)
        {
            var shadowAngle = -1 * _shadowAngle;

            double shadowDimension = _dimensionPercentage * marble.Width;

            var offsetX = shadowDimension * Math.Cos(shadowAngle * (Math.PI / 180));
            var offsetY = shadowDimension * Math.Sin(shadowAngle * (Math.PI / 180));


            return new PointF[] { removeShadowLine.Start, removeShadowLine.End, new PointF((float)(removeShadowLine.End.X + offsetX), (float)(removeShadowLine.End.Y + offsetY)), new PointF((float)(removeShadowLine.Start.X + offsetX), (float)(removeShadowLine.Start.Y + offsetY)) };
        }

        public PointD[] CreateShadowPathD(Line shadowLine, RectangleF marble, float forceOffsetYAbsValue)
        {
            double shadowDimension = forceOffsetYAbsValue / Math.Sin(_shadowAngle * (Math.PI / 180));

            var offsetX = shadowDimension * Math.Cos(_shadowAngle * (Math.PI / 180));
            var offsetY = shadowDimension * Math.Sin(_shadowAngle * (Math.PI / 180));


            return new PointD[] { new PointD(shadowLine.Start), new PointD(shadowLine.End), new PointD((shadowLine.End.X + offsetX), (shadowLine.End.Y + offsetY)), new PointD((shadowLine.Start.X + offsetX), (shadowLine.Start.Y + offsetY)) };
        }

        public PointF[] CreateShadowPathF(Line shadowLine, RectangleF marble, float forceOffsetYAbsValue)
        {
            double shadowDimension = forceOffsetYAbsValue / Math.Sin(_shadowAngle * (Math.PI / 180));

            var offsetX = shadowDimension * Math.Cos(_shadowAngle * (Math.PI / 180));
            var offsetY = shadowDimension * Math.Sin(_shadowAngle * (Math.PI / 180));


            return new PointF[] { shadowLine.Start, shadowLine.End, new PointF((float)(shadowLine.End.X + offsetX), (float)(shadowLine.End.Y + offsetY)), new PointF((float)(shadowLine.Start.X + offsetX), (float)(shadowLine.Start.Y + offsetY)) };
        }

        public PointD[] CreateAntiShadowPathD(Line removeShadowLine, RectangleF marble, float forceOffsetYAbsValue)
        {
            var shadowPath = CreateShadowPathD(removeShadowLine, marble, forceOffsetYAbsValue);

            var line1 = new LineD(shadowPath[0], shadowPath[2]);
            var line2 = new LineD(shadowPath[1], shadowPath[3]);

            var slope1 = line1.GetSlope();

            var xDiff1 = line1.End.X - line1.Start.X;
            var newPoint1 = new PointD(line1.Start.X - xDiff1, line1.Start.Y + (-xDiff1) * slope1.Value);

            var slope2 = line2.GetSlope();

            var xDiff2 = line2.End.X - line2.Start.X;
            var newPoint2 = new PointD(line2.Start.X - xDiff2, line2.Start.Y + (-xDiff2) * slope2.Value);

            return new PointD[] { line1.Start, line2.Start, newPoint2, newPoint1 };
        }
    }

    public class DigitShadowLinesCreator
    {
        public bool IncludeMarble { get; set; } = true;

        public float StrokeWidth { get; set; } = 0;

        private readonly ShadowCreator _shadowCreator;

        public DigitShadowLinesCreator(ShadowCreator shadowCreator)
        {
            _shadowCreator = shadowCreator;
        }

        public DigitShadowLinesCreator()
        {
            _shadowCreator = new ShadowCreator(0.1f, 45f);
        }

        public IEnumerable<PointF[]> CreateShadowPaths(RectangleF marble,
            List<DigitChiselResult> chiseledOutSections)
        {
            return CreateShadows(marble, chiseledOutSections)
                .Select(x => _shadowCreator.CreateShadowPath(x, marble));
        }

        public IEnumerable<Line> CreateShadows(RectangleF marble,
            List<DigitChiselResult> chiseledOutSections)
        {
            IEnumerable<DigitChiselResult> chiselResults = chiseledOutSections;
            if (IncludeMarble)
            {
                var marbleShadowsInfo = new DigitChiselResult(marble, RectangleSideName.Right, RectangleSideName.Bottom);
                chiselResults = chiselResults.Concat(new[] { marbleShadowsInfo });
            }

            var allLineInfos = chiselResults
                .SelectMany(x => x.ShadowsInfo.ShadowLineInfos)
                .ToList();

            var shadowLines = allLineInfos.Where(x => x.CastsShadow).ToList();
            var removeShadowLines = allLineInfos.Where(x => !x.CastsShadow).ToList();

            var result = GetUpdatedShadowLines(shadowLines.Select(x => new Line(x.Start, x.End)).ToList(), removeShadowLines.Select(x => new Line(x.Start, x.End)).ToList());
            result = JoinLineSegments(result.ToList());
            return AdjustShadowsForStroke(result.ToList(), shadowLines.Select(x => new Line(x.Start, x.End)).ToList(), removeShadowLines.Select(x => new Line(x.Start, x.End)).ToList(), marble);
        }

        private List<Line> FixUpMarbleEdgeShadowLinesForStrokeAdjustments(List<Line> removeShadowLines, RectangleF marble)
        {
            var updatedResults = new List<Line>(removeShadowLines);
            var lineDivider = new LineDivider();

            var topMarbleLine = new Line(marble.TopLeft(), marble.TopRight());
            updatedResults.Remove(topMarbleLine);

            var topLines = updatedResults.Where(x => x.Start.Y == marble.Top && x.End.Y == marble.Top).ToList();
            foreach (var topLine in topLines)
                updatedResults.Remove(topLine);

            if (topLines.Count == 0)
                updatedResults.Add(topMarbleLine);
            else
            {
                var loopResults = new List<Line>() { topMarbleLine };
                foreach (var topLine in topLines)
                {
                    if (loopResults.Count == 0)
                        break;

                    loopResults = loopResults
                        .SelectMany(x => lineDivider.DivideLine(x, topLine))
                        .ToList();
                }

                updatedResults.AddRange(loopResults);
            }

            return updatedResults;
        }

        private IEnumerable<Line> AdjustShadowsForStroke(List<Line> shadowLines, List<Line> originalShadowLines, List<Line> removeShadowLines, RectangleF marble)
        {
            if (StrokeWidth == 0)
                return shadowLines;

            removeShadowLines = FixUpMarbleEdgeShadowLinesForStrokeAdjustments(removeShadowLines, marble);
            
            var results = new List<Line>();
            var cache = new Dictionary<PointF, PointF>();
            foreach (var line in shadowLines)
            {                                                                                                                               
                var startConnection = FindConnectingLine(shadowLines, originalShadowLines, line, removeShadowLines, true);
                var endConnection = FindConnectingLine(shadowLines, originalShadowLines, line, removeShadowLines, false);

                PointF updatedStart;
                if (cache.ContainsKey(line.Start))
                    updatedStart = cache[line.Start];
                else
                {
                    cache[line.Start] = ShiftPointForStroke(startConnection, (line, true), line.Start, marble);
                    updatedStart = cache[line.Start];
                }

                PointF updatedEnd;
                if (cache.ContainsKey(line.End))
                    updatedEnd = cache[line.End];
                else
                {
                    cache[line.End] = ShiftPointForStroke((line, true), endConnection, line.End, marble);
                    updatedEnd = cache[line.End];
                }
                
                results.Add(new Line(updatedStart, updatedEnd));
            }

            return results;
        }

        private PointF ShiftPointForStroke((Line Line, bool IsShadow) startConnection, (Line Line, bool IsShadow) endConnection, PointF intersectionPoint, RectangleF marble)
        {
            var lineWithKnownShadow = startConnection.IsShadow
                ? startConnection.Line
                : endConnection.Line;

            var otherConnection = startConnection.IsShadow
                ? endConnection
                : startConnection;

            var otherLine = otherConnection.Line;

            var shadowPath = _shadowCreator.CreateShadowPathF(lineWithKnownShadow, marble, StrokeWidth / 2);
            var shadowOutline = new Line(shadowPath[2], shadowPath[3]);

            var lines = GetLines(otherLine, StrokeWidth / 2);

            if (IsValidLine(lines.Line1, shadowOutline, lineWithKnownShadow, otherLine))
                return lines.Line1.GetIntersectionPointWith(shadowOutline);
            else
                return lines.Line2.GetIntersectionPointWith(shadowOutline);
        }

        private bool IsValidLine(Line lineToTest, Line shadowOutline, Line knownShadowLine, Line otherLine)
        {
            var junction = lineToTest.GetIntersectionPointWith(shadowOutline);

            var lineToTestSegment = Line.CreateFromPoints(lineToTest.Points.Concat(new[] { junction }).ToArray());
            var shadowOutlineSegment = Line.CreateFromPoints(shadowOutline.Points.Concat(new[] { junction }).ToArray());

            return !lineToTestSegment.IntersectsAsLineSegment(knownShadowLine) && !shadowOutlineSegment.IntersectsAsLineSegment(otherLine);
        }

        private bool IsGreaterThanLine(Line line, Line lineToCheck)
        {
            var slope = line.GetSlope();
            if (slope == null)
                return lineToCheck.Start.X > line.Start.X;
            else if (slope == 0)
                return lineToCheck.Start.Y > line.Start.Y;
            else
            {
                var b = line.GetB();
                if (b == null)
                    throw new NotSupportedException("This shouldn't happen");

                var lineY = slope.Value * lineToCheck.Start.X + b.Value;
                var result = lineToCheck.Start.Y > lineY;
                return slope.Value >= 0 ? result : (!result);
            }
        }

        public (Line Line1, Line Line2) GetLines(Line line, float distance)
        {
            var slope = line.GetSlope();
            var inverseSlope = slope == null ? 0 : (-1 / slope.Value);

            var inverseAngle = Math.Atan(inverseSlope);
            var xDiff = distance * Math.Cos(inverseAngle);
            var yDiff = distance * Math.Sin(inverseAngle);

            var diff1 = new Line(new PointF((float)(line.Start.X + xDiff), (float)(line.Start.Y + yDiff)), new PointF((float)(line.End.X + xDiff), (float)(line.End.Y + yDiff)));
            var diff2 = new Line(new PointF((float)(line.Start.X - xDiff), (float)(line.Start.Y - yDiff)), new PointF((float)(line.End.X - xDiff), (float)(line.End.Y - yDiff)));

            return (diff1, diff2);
        }

        private Line GetShadowLine(Line line, float distance)
        {
            var slope = line.GetSlope();
            var inverseSlope = slope == null ? 0 : (-1 / slope.Value);

            var inverseAngle = Math.Atan(inverseSlope);
            var xDiff = distance * Math.Cos(inverseAngle);
            var yDiff = distance * Math.Sin(inverseAngle);

            var diff1 = new Line(new PointF((float)(line.Start.X + xDiff), (float)(line.Start.Y + yDiff)), new PointF((float)(line.End.X + xDiff), (float)(line.End.Y + yDiff)));
            return diff1;
        }

        private (Line Line, bool IsShadow) FindConnectingLine(List<Line> shadowLines, List<Line> originalShadowLines, Line targetLine, List<Line> removeShadowLines, bool useStartPoint)
        {
            removeShadowLines = CleanUpExtraShadowLines(originalShadowLines, removeShadowLines);

            var point = useStartPoint ? targetLine.Start : targetLine.End;
            var targetSlope = targetLine.GetSlope();
            foreach (var line in shadowLines.Select(x => new { Line = x, IsShadow = true }).Where(x => x.Line != targetLine).Concat(removeShadowLines.Select(x => new { Line = x, IsShadow = false })))
            {
                if (line.Line.Points.Contains(point) && targetSlope != line.Line.GetSlope())
                    return (line.Line, line.IsShadow);
            }

            throw new Exception("This shouldn't happen");
        }

        private List<Line> CleanUpExtraShadowLines(List<Line> originalShadowLines, List<Line> removeShadowLines)
        {
            originalShadowLines = originalShadowLines.Distinct().ToList();
            removeShadowLines = removeShadowLines.Distinct().ToList();

            var results = new List<Line>();
            foreach (var removeShadowLine in removeShadowLines)
            {
                var subResults = new List<Line>();
                foreach (var originalShadowLine in originalShadowLines)
                {
                    if (removeShadowLine.TryGetLeftoverPart(originalShadowLine, out var partial))
                    {
                        subResults.Add(partial);
                    }
                }

                results.AddRange(subResults);
                if (!subResults.Any())
                    results.Add(removeShadowLine);
            }

            return results;
        }

        private IEnumerable<Line> JoinLineSegments(List<Line> lines)
        {
            var currentIndex = 0;
            while (currentIndex < lines.Count)
            {
                var currentLine = lines[currentIndex];
                var slope = currentLine.GetSlope();
                for (int i = lines.Count - 1; i > currentIndex; i--)
                {
                    var nextLine = lines[i];
                    var nextSlope = nextLine.GetSlope();

                    if (slope == nextSlope)
                    {
                        var allPoints = currentLine.Points.Concat(nextLine.Points)
                            .Select(x => new { Point = x, ParametricValue = currentLine.GetParametericValue(x) })
                            .OrderBy(x => x.ParametricValue)
                            .ToList();

                        if (allPoints.GroupBy(x => x).Any(x => x.Count() > 1))
                        {
                            currentLine = new Line(allPoints.First().Point, allPoints.Last().Point);
                            lines.RemoveAt(i);
                        }
                    }
                }

                yield return currentLine;
                currentIndex++;
            }
        }

        private IEnumerable<Line> GetUpdatedShadowLines(IEnumerable<Line> originalShadowLines, IEnumerable<Line> removeLines)
        {
            var lineDivider = new LineDivider();

            var results = new List<Line>();
            foreach (var shadowLine in originalShadowLines)
            {
                var loopResults = new List<Line>() { shadowLine };
                foreach (var removeLine in removeLines)
                {
                    if (loopResults.Count == 0)
                        break;

                    loopResults = loopResults
                        .SelectMany(x => lineDivider.DivideLine(x, removeLine))
                        .ToList();
                }

                results.AddRange(loopResults);
            }

            return results;
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
            foreach (var pair in points.Zip(points.Skip(1).Concat(new[] {points.First()}), (x, y) => new { Start = x, End = y }))
            {
                items.Add(new ShadowLineInfo(pair.Start, pair.End, isShadowSide[sideIndex]));
                sideIndex++;
            }

            ShadowsInfo = new ChiselShadowsInfo(items);
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
                }, false, false, false);
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
            }, true, false, false, ExtendLeft ? false : true);
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
                    .ToArray(), result.ShadowsInfo.ShadowLineInfos.Select(x => x.CastsShadow).ToArray());
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
                    : new[] { false, false, false };

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
                    ? new[] { false, false, false }
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
