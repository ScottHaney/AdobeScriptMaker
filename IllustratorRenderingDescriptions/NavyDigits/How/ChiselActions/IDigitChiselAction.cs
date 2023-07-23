using Geometry;
using Geometry.Lines;
using Geometry.LineSegments;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;

namespace IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions
{
    public interface IDigitChisleAction
    {
        IEnumerable<DigitChiselResult> GetPoints(RectangleF outerBounds);
    }

    public class DigitChiselResult
    {
        public readonly PointD[] Points;
        public readonly List<ChiselEdge> Edges;

        public DigitChiselResult(PointF[] points,
            params ChiselEdgeInfo[] edgesInfo)
        {
            Points = points
                .Select(x => new PointD(x))
                .ToArray();

            var edges = new List<ChiselEdge>();
            var sideIndex = 0;
            foreach (var pair in points.Zip(points.Skip(1).Concat(new[] { points.First() }), (x, y) => new { Start = x, End = y }))
            {
                edges.Add(new ChiselEdge(pair.Start, pair.End, edgesInfo[sideIndex]));
                sideIndex++;
            }

            Edges = edges;
        }

        public DigitChiselResult(PointD[] points,
            List<ChiselEdge> edges)
        {
            Points = points;
            Edges = edges;
        }

        public DigitChiselResult ShiftY(double yValue)
        {
            var updatedPoints = Points.Select(x => new PointD(x.X, x.Y + yValue)).ToArray();
            var updatedEdges = Edges.Select(x => new ChiselEdge(x.LineSegment.ShiftY(yValue), x.EdgeInfo)).ToList();

            return new DigitChiselResult(updatedPoints, updatedEdges);
        }

        public DigitChiselResult(RectangleF rect,
            params ChiselEdgeInfo[] edgesInfo)
            : this(rect.ToPathPoints(), edgesInfo)
        { }

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

    public class ChiselEdge
    {
        public readonly LineSegment LineSegment;
        public readonly ChiselEdgeInfo EdgeInfo;

        public ChiselEdge(PointF start,
            PointF end,
            ChiselEdgeInfo edgeInfo)
        {
            var factory = new LineSegmentRepresentationFactory(new LineRepresentationFactory());

            LineSegment = factory.Create(new PointD(start), new PointD(end));
            EdgeInfo = edgeInfo;
        }

        public ChiselEdge(LineSegment lineSegment,
            ChiselEdgeInfo edgeInfo)
        {
            LineSegment = lineSegment;
            EdgeInfo = edgeInfo;
        }

        public override string ToString()
        {
            return $"Line Segment: {LineSegment}, IsMarble: {EdgeInfo.CreatesMarbleEdge}, IsShadow: {EdgeInfo.CastsShadow}, Orientation: {EdgeInfo.MarbleOrientation}";
        }
    }

    public class ChiselEdgeInfo : IEquatable<ChiselEdgeInfo>
    {
        public readonly bool CastsShadow;
        public readonly bool CreatesMarbleEdge;
        public readonly MarbleOrientations? MarbleOrientation;

        public ChiselEdgeInfo(bool castsShadow,
            bool createsMarbleEdge)
        {
            if (castsShadow && !createsMarbleEdge)
                throw new Exception("This can't happen. Only a marble edge can cast a shadow!");

            CastsShadow = castsShadow;
            CreatesMarbleEdge = createsMarbleEdge;

            if (createsMarbleEdge)
                MarbleOrientation = CastsShadow ? MarbleOrientations.Negative : MarbleOrientations.Positive;
        }

        public ChiselEdgeInfo(bool castsShadow,
            MarbleOrientations? marbleOrientation)
        {
            CastsShadow = castsShadow;
            CreatesMarbleEdge = true;
            MarbleOrientation = marbleOrientation;
        }

        public static bool operator ==(ChiselEdgeInfo lhs, ChiselEdgeInfo rhs)
        {
            if (ReferenceEquals(lhs, null))
                return ReferenceEquals(rhs, null);

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ChiselEdgeInfo lhs, ChiselEdgeInfo rhs)
            => !(lhs == rhs);

        public bool Equals(ChiselEdgeInfo other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return CastsShadow == other.CastsShadow && CreatesMarbleEdge == other.CreatesMarbleEdge && MarbleOrientation == other.MarbleOrientation;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ChiselEdgeInfo);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return CastsShadow.GetHashCode() + CreatesMarbleEdge.GetHashCode() + MarbleOrientation?.GetHashCode() ?? 3;
            }
        }
    }

    public enum MarbleOrientations
    {
        Positive,
        Negative
    }

    public enum DigitChiselLocation
    {
        Left,
        Center,
        Right
    }
}
