using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;

namespace Geometry
{
    public class Line : IEquatable<Line>
    {
        public readonly PointF Start;
        public readonly PointF End;

        public Line(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }

        public float? GetSlope()
        {
            if (Start.X == End.X)
                return null;
            else
            {
                return (End.Y - Start.Y) / (End.X - Start.X);
            }
        }

        public float? GetParametericValue(PointF targetPoint)
        {
            var slope = GetSlope();

            if (slope == null)
            {
                if (Start.X == targetPoint.X)
                    return targetPoint.Y - Start.Y;
                else
                    return null;
            }
            else
            {
                var xDiff = targetPoint.X - Start.X;
                var expectedYValue = Start.Y + slope * xDiff;

                if (expectedYValue == targetPoint.Y)
                    return xDiff;
                else
                    return null;
            }
        }

        public override string ToString()
        {
            return $"[({Start.X},{Start.Y}), ({End.X},{End.Y})]";
        }

        public static bool operator==(Line line1, Line line2)
        {
            if (ReferenceEquals(line1, null))
                return ReferenceEquals(line2, null);

            return line1.Equals(line2);
        }

        public static bool operator!=(Line line1, Line line2)
            => !(line1 == line2);

        public bool Equals([AllowNull] Line other)
        {
            if (ReferenceEquals(other, null))
                return false;

            var points = new[] { Start, End }.OrderBy(x => x.X);
            var otherPoints = new[] { other.Start, other.End }.OrderBy(x => x.X);

            return points.SequenceEqual(otherPoints);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Line);
        }

        public override int GetHashCode()
        {
            checked
            {
                return Start.GetHashCode() + End.GetHashCode();
            }
        }
    }
}
