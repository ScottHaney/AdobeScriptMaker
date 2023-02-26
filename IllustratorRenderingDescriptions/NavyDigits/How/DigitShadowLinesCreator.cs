using Geometry.Lines;
using Geometry.LineSegments;
using IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How
{
    public class DigitShadowLinesCreator
    {
        private readonly ShadowCreator _shadowCreator;

        public bool IncludeMarble { get; set; } = true;
        public float StrokeWidth { get; set; } = 1;

        public DigitShadowLinesCreator(ShadowCreator shadowCreator)
        {
            _shadowCreator = shadowCreator;
        }

        public DigitShadowLinesCreator()
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

        private IEnumerable<ChiselEdge> JoinMarbleEdges(List<ChiselEdge> marbleEdges, LineSegmentRepresentationFactory factory)
        {
            foreach (var group in marbleEdges.GroupBy(x => x.EdgeInfo))
            {
                var joinedSegments = LineSegment.Join(group.Select(x => x.LineSegment).ToArray());
                foreach (var joinedSegment in joinedSegments)
                    yield return new ChiselEdge(joinedSegment, group.Key);
            }
        }

        private IEnumerable<LineSegment> AdjustShadowsForStroke3(List<ChiselEdge> marbleLines, LineSegmentRepresentationFactory lineSegmentFactory)
        {
            foreach (var marbleLine in marbleLines.Where(x => x.EdgeInfo.CastsShadow))
            {
                var startConnection = marbleLines
                    .Where(x => x.LineSegment != marbleLine.LineSegment)
                    .Single(x => marbleLine.LineSegment.StartPoint == x.LineSegment.StartPoint || marbleLine.LineSegment.StartPoint == x.LineSegment.EndPoint);

                var endConnection = marbleLines
                    .Where(x => x.LineSegment != marbleLine.LineSegment)
                    .Single(x => marbleLine.LineSegment.EndPoint == x.LineSegment.StartPoint || marbleLine.LineSegment.EndPoint == x.LineSegment.EndPoint);

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
    }
}
