using AdobeComponents.Animation;
using AdobeComponents.Components;
using Geometry;
using Geometry.Lines;
using Geometry.LineSegments;
using IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions;
using IllustratorRenderingDescriptions.NavyDigits.What;
using RenderingDescriptions.How;
using RenderingDescriptions.Timing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
            var strokeWidth = 20;

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
                    new DigitHole(DigitHoleName.Bottom, widthPaddingPercentage),
                    new DigitHole(DigitHoleName.Top, widthPaddingPercentage),
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

    public class DigitShadowLinesCreator2
    {
        private readonly ShadowCreator _shadowCreator;

        public bool IncludeMarble { get; set; } = true;
        public float StrokeWidth { get; set; } = 1;

        public DigitShadowLinesCreator2(ShadowCreator shadowCreator)
        {
            _shadowCreator = shadowCreator;
        }

        public DigitShadowLinesCreator2()
        {
            _shadowCreator = new ShadowCreator(0.2f, 45);
        }

        public IEnumerable<PointF[]> CreateShadowPaths(RectangleF marble,
            List<DigitChiselResult> chiseledOutSections)
        {
            return CreateShadows(marble, chiseledOutSections)
                .Select(x => _shadowCreator.CreateShadowPath(x.ToLegacyLine(), marble));
        }

        public IEnumerable<LineSegment> CreateShadows(RectangleF marble,
            List<DigitChiselResult> chiseledOutSections)
        {
            var chiselResults = chiseledOutSections;
            if (IncludeMarble)
            {
                var marbleShadowsInfo = new DigitChiselResult(marble, new ChiselEdgeInfo(false, true), new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(true, true), new ChiselEdgeInfo(false, true));
                chiselResults = new[] { marbleShadowsInfo }.Concat(chiselResults).ToList();
            }

            var lineSegmentFactory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());

            var marbleEdges = chiselResults.First().Edges.Where(x => x.EdgeInfo.CreatesMarbleEdge).ToList();
            foreach (var chiselResult in chiselResults.Skip(1))
            {
                /*var newMarbleEdges = chiselResult.Edges
                    .Where(x => IsNewMarbleEdge(marbleEdges, x))
                    .ToList();*/

                var newMarbleEdges = GetNewMarbleEdges(marbleEdges, chiselResult.Edges);

                var trimmedEdges = TrimMarbleEdges(marbleEdges, chiselResult.Edges);

                marbleEdges = trimmedEdges.Concat(newMarbleEdges).ToList();
            }

            marbleEdges = JoinMarbleEdges(marbleEdges, lineSegmentFactory).ToList();
            return AdjustShadowsForStroke3(marbleEdges, lineSegmentFactory).ToList();
        }

        private IEnumerable<ChiselEdge> TrimMarbleEdges(IEnumerable<ChiselEdge> currentEdges,
            IEnumerable<ChiselEdge> chiselResultEdges)
        {
            var edgesToRemove = chiselResultEdges.Select(x => x.LineSegment).ToArray();
            foreach (var currentEdge in currentEdges)
            {
                foreach (var item in currentEdge.LineSegment.Exclude(edgesToRemove))
                    yield return new ChiselEdge(item, currentEdge.EdgeInfo);
            }
        }

        private IEnumerable<ChiselEdge> GetNewMarbleEdges(IEnumerable<ChiselEdge> currentMarbleEdges,
            IEnumerable<ChiselEdge> chiselEdges)
        {
            var currentMarble = currentMarbleEdges.Select(x => x.LineSegment).ToArray();
            foreach (var chiselMarbleEdge in chiselEdges.Where(x => x.EdgeInfo.CreatesMarbleEdge))
            {
                foreach (var item in chiselMarbleEdge.LineSegment.Exclude(currentMarble))
                    yield return new ChiselEdge(item, chiselMarbleEdge.EdgeInfo);
            }
        }

        private bool IsNewMarbleEdge(IEnumerable<ChiselEdge> currentEdges,
            ChiselEdge newEdge)
        {
            if (newEdge.EdgeInfo.CreatesMarbleEdge)
                return currentEdges.All(x => !x.LineSegment.OverlapsWith(newEdge.LineSegment));
            else
                return false;
        }

        private List<LineSegment> GetUpdatedShadowLines(IEnumerable<LineSegment> originalShadowLines, IEnumerable<LineSegment> removeLines)
        {
            var lineDivider = new LineDivider2();
            return originalShadowLines.SelectMany(x => lineDivider.DivideLines(x, removeLines)).ToList();
        }

        private IEnumerable<ChiselEdge> JoinMarbleEdges(List<ChiselEdge> marbleEdges, LineSegmentRepresentationFactory factory)
        {
            foreach (var group in marbleEdges.GroupBy(x => x.EdgeInfo))
            {
                var joinedSegments = LineSegment.Join(group.Select(x => x.LineSegment).ToArray());
                foreach (var joinedSegment in joinedSegments)
                    yield return new ChiselEdge(joinedSegment, group.Key);
            }
        }

        private IEnumerable<LineSegment> JoinLineSegments(List<LineSegment> segments, LineSegmentRepresentationFactory lineSegmentFactory)
        {
            if (!segments.Any())
                yield break;

            var groups = segments.GroupBy(x => x, new LineSegmentLineEqualityComparer()).ToList();
            foreach (var group in groups)
            {
                var ranges = group
                    .Select(x => new { Segment = x, Range = x.GetParametricRange() })
                    .OrderBy(x => x.Range.Start.ParametricValue)
                    .ToList();

                var commonLineRep = group.First().ToLine();

                var currentRange = ranges.First().Range;
                for (int i = 1; i < ranges.Count; i++)
                {
                    var range = ranges[i].Range;
                    if (currentRange.OverlapsOrConnectsWith(range))
                        currentRange = new ParametricRange(currentRange.Start, range.End.ParametricValue > currentRange.End.ParametricValue ? range.End : currentRange.End);
                    else
                    {
                        yield return lineSegmentFactory.Create(commonLineRep, currentRange.Start.Point, currentRange.End.Point);
                        currentRange = range;
                    }
                }

                yield return lineSegmentFactory.Create(commonLineRep, currentRange.Start.Point, currentRange.End.Point);
            }
        }

        private IEnumerable<LineSegment> AdjustShadowsForStroke3(List<ChiselEdge> marbleLines, LineSegmentRepresentationFactory lineSegmentFactory)
        {
            foreach (var marbleLine in marbleLines.Where(x => x.EdgeInfo.CastsShadow))
            {
                var startConnection = marbleLines
                    .Where(x => x.LineSegment != marbleLine.LineSegment)
                    .Single(x => marbleLine.LineSegment.StartPoint == x.LineSegment.StartPoint || marbleLine.LineSegment.StartPoint == x.LineSegment.EndPoint);

                var endConnections = marbleLines
                    .Where(x => x.LineSegment != marbleLine.LineSegment)
                    .Where(x => marbleLine.LineSegment.EndPoint == x.LineSegment.StartPoint || marbleLine.LineSegment.EndPoint == x.LineSegment.EndPoint)
                    .ToList();
                                    
                var endConnection = endConnections.Single();

                var lineBars = marbleLine.LineSegment.ToLine().GetParallelBoundingLines(StrokeWidth / 2);
                LineRepresentation shadowLine;
                if (marbleLine.EdgeInfo.MarbleOrientation == MarbleOrientations.Negative)
                    shadowLine = lineBars.First(x => x.Direction == RelativeLineDirection.AddTo).Line;
                else
                    shadowLine = lineBars.First(x => x.Direction == RelativeLineDirection.SubtractedFrom).Line;

                var startBars = startConnection.LineSegment.ToLine().GetParallelBoundingLines(StrokeWidth / 2);
                LineRepresentation startConnectionLine;
                if (startConnection.EdgeInfo.MarbleOrientation == MarbleOrientations.Negative)
                    startConnectionLine = startBars.First(x => x.Direction == RelativeLineDirection.AddTo).Line;
                else
                    startConnectionLine = startBars.First(x => x.Direction == RelativeLineDirection.SubtractedFrom).Line;

                var endBars = endConnection.LineSegment.ToLine().GetParallelBoundingLines(StrokeWidth / 2);
                LineRepresentation endConnectionLine;
                if (endConnection.EdgeInfo.MarbleOrientation == MarbleOrientations.Negative)
                    endConnectionLine = endBars.First(x => x.Direction == RelativeLineDirection.AddTo).Line;
                else
                    endConnectionLine = endBars.First(x => x.Direction == RelativeLineDirection.SubtractedFrom).Line;

                var point1 = shadowLine.GetIntersectionWith(startConnectionLine).GetStart().Value;
                var point2 = shadowLine.GetIntersectionWith(endConnectionLine).GetStart().Value;

                yield return lineSegmentFactory.Create(point1, point2);
            }
        }

        /*private IEnumerable<ChiselEdge> JoinLineSegments2(List<ChiselEdge> segments, LineSegmentRepresentationFactory lineSegmentFactory)
        {
            if (!segments.Any())
                yield break;

            var groups = segments
                .Select(x => new { EdgeInfo = x.EdgeInfo, Segment = lineSegmentFactory.Create(new PointD(x.Start), new PointD(x.End)) })
                .GroupBy(x => x.Segment, new LineSegmentLineEqualityComparer()).ToList();

            foreach (var group in groups)
            {
                var ranges = group
                    .Select(x => new { Segment = x.Segment, Range = x.Segment.GetParametricRange() })
                    .OrderBy(x => x.Range.Start.ParametricValue)
                    .ToList();

                var commonLineRep = group.First().Segment.ToLine();

                var currentRange = ranges.First().Range;
                for (int i = 1; i < ranges.Count; i++)
                {
                    var range = ranges[i].Range;
                    if (currentRange.TryConnectOtherRange(range, out var combinedRange))
                    {
                        var rangePoints = new[] { combinedRange.Start.Point, combinedRange.End.Point };

                        var mergePoint = new[] { range.Start.Point, range.End.Point }.Where(x => !rangePoints.Contains(x)).Single();
                        var appearanceCount = segments.Count(x => new PointD(x.Start) == mergePoint || new PointD(x.End) == mergePoint);
                        if (appearanceCount == 2)
                        {
                            currentRange = new ParametricRange(currentRange.Start, range.End.ParametricValue > currentRange.End.ParametricValue ? range.End : currentRange.End);
                        }
                        else
                        {
                            yield return new ChiselEdge(currentRange.Start.Point.ToPointF(), currentRange.End.Point.ToPointF(), group.First().EdgeInfo);
                            currentRange = range;
                        }
                    }
                    else if (currentRange.OverlapsOrConnectsWith(range))
                    {
                        currentRange = new ParametricRange(currentRange.Start, range.End.ParametricValue > currentRange.End.ParametricValue ? range.End : currentRange.End);
                    }
                    else
                    {
                        yield return new ChiselEdge(currentRange.Start.Point.ToPointF(), currentRange.End.Point.ToPointF(), group.First().EdgeInfo);
                        currentRange = range;
                    }
                }

                yield return new ChiselEdge(currentRange.Start.Point.ToPointF(), currentRange.End.Point.ToPointF(), group.First().EdgeInfo);
            }
        }*/

        /*private IEnumerable<LineSegment> AdjustShadowsForStroke2(List<ChiselEdge> marbleLines, LineSegmentRepresentationFactory lineSegmentFactory)
        {
            var marbleLinesInfo = marbleLines
                .Select(x => new { EdgeInfo = x.EdgeInfo, LineSegment = lineSegmentFactory.Create(new PointD(x.Start), new PointD(x.End)) })
                .ToList();

            foreach (var marbleLineInfo in marbleLinesInfo.Where(x => x.EdgeInfo.CastsShadow))
            {
                var startConnection = marbleLinesInfo
                    .Where(x => x.LineSegment != marbleLineInfo.LineSegment)
                    .Single(x => marbleLineInfo.LineSegment.StartPoint == x.LineSegment.StartPoint || marbleLineInfo.LineSegment.StartPoint == x.LineSegment.EndPoint);

                var endConnections = marbleLinesInfo
                    .Where(x => x.LineSegment != marbleLineInfo.LineSegment)
                    .Where(x => marbleLineInfo.LineSegment.EndPoint == x.LineSegment.StartPoint || marbleLineInfo.LineSegment.EndPoint == x.LineSegment.EndPoint)
                    .ToList();
                                    
                var endConnection = endConnections.Single();

                var lineBars = marbleLineInfo.LineSegment.ToLine().GetParallelBoundingLines(StrokeWidth / 2);
                LineRepresentation shadowLine;
                if (marbleLineInfo.EdgeInfo.MarbleOrientation == MarbleOrientations.Negative)
                    shadowLine = lineBars.First(x => x.Direction == RelativeLineDirection.AddTo).Line;
                else
                    shadowLine = lineBars.First(x => x.Direction == RelativeLineDirection.SubtractedFrom).Line;

                var startBars = startConnection.LineSegment.ToLine().GetParallelBoundingLines(StrokeWidth / 2);
                LineRepresentation startConnectionLine;
                if (startConnection.EdgeInfo.MarbleOrientation == MarbleOrientations.Negative)
                    startConnectionLine = startBars.First(x => x.Direction == RelativeLineDirection.AddTo).Line;
                else
                    startConnectionLine = startBars.First(x => x.Direction == RelativeLineDirection.SubtractedFrom).Line;

                var endBars = endConnection.LineSegment.ToLine().GetParallelBoundingLines(StrokeWidth / 2);
                LineRepresentation endConnectionLine;
                if (endConnection.EdgeInfo.MarbleOrientation == MarbleOrientations.Negative)
                    endConnectionLine = endBars.First(x => x.Direction == RelativeLineDirection.AddTo).Line;
                else
                    endConnectionLine = endBars.First(x => x.Direction == RelativeLineDirection.SubtractedFrom).Line;

                var point1 = shadowLine.GetIntersectionWith(startConnectionLine).GetStart().Value;
                var point2 = shadowLine.GetIntersectionWith(endConnectionLine).GetStart().Value;

                yield return lineSegmentFactory.Create(point1, point2);
            }
        }*/

        private IEnumerable<LineSegment> AdjustShadowsForStroke(List<LineSegment> shadowLines, List<LineSegment> removeShadowLines, RectangleF marble, LineSegmentRepresentationFactory lineSegmentFactory)
        {
            if (StrokeWidth == 0)
                return shadowLines;

            var sortedRemoveShadowLines = removeShadowLines.OrderByDescending(x => x.GetLength()).ToList();
            var fixedShadowLines = new List<LineSegment>();
            var divider = new LineDivider2();
            for (int i = 0; i < sortedRemoveShadowLines.Count; i++)
            {
                var target = sortedRemoveShadowLines[i];
                fixedShadowLines.AddRange(divider.DivideLines(target, sortedRemoveShadowLines.Skip(i + 1)));
            }

            throw new NotImplementedException();

            /*var results = new List<LineSegment>();
            foreach (var line in shadowLines)
            {
                var startConnection = FindConnectingLine(line, line.StartPoint, shadowLines, fixedShadowLines);
                var endConnection = FindConnectingLine(line, line.EndPoint, shadowLines, fixedShadowLines);

                var lineBars = line.ToLine().GetParallelBoundingLines(StrokeWidth / 2);
                var shadowBar = lineBars.First(x => x.Direction == RelativeLineDirection.Above || x.Direction == RelativeLineDirection.Right || x.Direction == RelativeLineDirection.GreaterThan);
                var otherBar = lineBars.SkipWhile(x => x.Direction == RelativeLineDirection.Above || x.Direction == RelativeLineDirection.Right || x.Direction == RelativeLineDirection.GreaterThan).First();

                var startBar = GetMatchingLine(shadowBar, otherBar, line, startConnection.Line);
                var endBar = GetMatchingLine(shadowBar, otherBar, line, endConnection.Line);

                var newStartPoint = shadowBar.Line.GetIntersectionWith(startBar.Line).GetStart().Value;
                var newEndPoint = shadowBar.Line.GetIntersectionWith(endBar.Line).GetStart().Value;

                results.Add(lineSegmentFactory.Create(newStartPoint, newEndPoint));
            }

            return results;*/
        }

        private ParallelBoundingLine GetMatchingLine(ParallelBoundingLine shadowBar, ParallelBoundingLine otherBar, LineSegment shadowLine, LineSegment connectingLine)
        {
            var uniquePoints = new[] { shadowLine.StartPoint, shadowLine.EndPoint }.Concat(new[] { connectingLine.StartPoint, connectingLine.EndPoint }).Distinct().ToList();

            if (uniquePoints.Count != 3)
                throw new Exception("This shouldn't happen");

            var centerOfMass = new PointD(uniquePoints.Sum(x => x.X) / uniquePoints.Count, uniquePoints.Sum(x => x.Y) / uniquePoints.Count);

            var connectingLineBars = connectingLine.ToLine().GetParallelBoundingLines(StrokeWidth / 2);
            var bar1 = connectingLineBars.First();
            var bar2 = connectingLineBars.Last();

            var shadowBarDist = shadowBar.Line.DistanceToPoint(centerOfMass);
            var otherBarDist = otherBar.Line.DistanceToPoint(centerOfMass);
            var bar1Dist = bar1.Line.DistanceToPoint(centerOfMass);
            var bar2Dist = bar2.Line.DistanceToPoint(centerOfMass);

            var returnCloserBar = shadowBarDist < otherBarDist;
            if (returnCloserBar)
                return bar1Dist < bar2Dist ? bar1 : bar2;
            else
                return bar1Dist > bar2Dist ? bar1 : bar2;
        }

        private ParallelBoundingLine GetBoundingLine(LineSegment connection, PointD targetPoint)
        {
            var otherPoint = connection.StartPoint == targetPoint
                ? connection.EndPoint
                : connection.StartPoint;

            var bars = connection.ToLine().GetParallelBoundingLines(StrokeWidth / 2);
            bool useShadowBar;
            if (otherPoint.X <= targetPoint.X && otherPoint.Y <= targetPoint.Y)
                useShadowBar = true;
            else if (otherPoint.X >= targetPoint.X && otherPoint.Y <= targetPoint.Y)
                useShadowBar = false;
            else if (otherPoint.X >= targetPoint.X && otherPoint.Y >= targetPoint.Y)
                useShadowBar = false;
            else
                useShadowBar = false;

            throw new NotImplementedException();
            /*if (useShadowBar)
                return bars.First(x => x.Direction == RelativeLineDirection.Above || x.Direction == RelativeLineDirection.Right || x.Direction == RelativeLineDirection.GreaterThan);
            else
                return bars.SkipWhile(x => x.Direction == RelativeLineDirection.Above || x.Direction == RelativeLineDirection.Right || x.Direction == RelativeLineDirection.GreaterThan).First();*/
        }

        private (LineSegment Line, bool IsShadow) FindConnectingLine(LineSegment targetLine, PointD targetPoint, List<LineSegment> shadowLines, List<LineSegment> removeShadowLines)
        {
            foreach (var option in shadowLines.Where(x => x != targetLine))
            {
                if (targetLine.TryJoinWith(option, out var matchingPoints) && matchingPoints.Length == 1 && matchingPoints.First() == targetPoint)
                    return (option, true);
            }

            foreach (var option in removeShadowLines)
            {
                if (targetLine.TryJoinWith(option, out var matchingPoints) && matchingPoints.Length == 1 && matchingPoints.First() == targetPoint)
                {
                    //Make sure to avoid a co-linear line
                    if (!option.ToLine().GetIntersectionWith(targetLine.ToLine()).IsTheSameLine)
                        return (option, false);
                }
            }

            throw new Exception("This shouldn't happen");
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
}
