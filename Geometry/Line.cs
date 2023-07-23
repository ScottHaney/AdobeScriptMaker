using Geometry.Lines;
using Geometry.LineSegments;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Geometry
{
    public class Line : IEquatable<Line>
    {
        public readonly PointF Start;
        public readonly PointF End;

        public PointF[] Points => new[] { Start, End };

        public Line(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }

        public LineSegment ToLineSegment()
        {
            return new LineSegment(new TwoPointLineRepresentation(new PointD(Start), new PointD(End)), new PointD(Start), new PointD(End));
        }

        public static Line CreateFromPoints(params PointF[] points)
        {
            if (points.Length < 2)
                throw new NotSupportedException();

            PointF[] sortedPoints;
            if (points.Select(x => x.X).Distinct().Count() == 1)
                sortedPoints = points.OrderBy(x => x.Y).ToArray();
            else
                sortedPoints = points.OrderBy(x => x.X).ToArray();

            return new Line(sortedPoints.First(), sortedPoints.Last());
        }

        public bool IntersectsAsLineSegment(Line otherLine)
        {
            var intersectionPoint = GetIntersectionPointWith(otherLine);

            var slope = GetSlope();
            var otherSlope = otherLine.GetSlope();

            if (slope != null && otherSlope != null)
            {
                var otherMinX = otherLine.Points.Select(x => x.X).Min();
                var otherMaxX = otherLine.Points.Select(x => x.X).Max();
                var currentMinX = Points.Select(x => x.X).Min();
                var currentMaxX = Points.Select(x => x.X).Max();

                return (intersectionPoint.X >= otherMinX && intersectionPoint.X <= otherMaxX) && (intersectionPoint.X >= currentMinX && intersectionPoint.X <= currentMaxX);
            }
            else if (slope == null)
            {
                var otherMinX = otherLine.Points.Select(x => x.X).Min();
                var otherMaxX = otherLine.Points.Select(x => x.X).Max();
                var currentMinY = Points.Select(x => x.Y).Min();
                var currentMaxY = Points.Select(x => x.Y).Max();

                return (intersectionPoint.X >= otherMinX && intersectionPoint.X <= otherMaxX) && (intersectionPoint.Y >= currentMinY && intersectionPoint.Y <= currentMaxY);
            }
            else
            {
                var otherMinY = otherLine.Points.Select(x => x.Y).Min();
                var otherMaxY = otherLine.Points.Select(x => x.Y).Max();
                var currentMinX = Points.Select(x => x.X).Min();
                var currentMaxX = Points.Select(x => x.X).Max();

                return (intersectionPoint.Y >= otherMinY && intersectionPoint.Y <= otherMaxY) && (intersectionPoint.X >= currentMinX && intersectionPoint.X <= currentMaxX);
            }
        }

        public bool TryGetLeftoverPart(Line otherLine, out Line result)
        {
            var slope = GetSlope();
            var otherSlope = otherLine.GetSlope();

            if (slope == otherSlope)
            {
                var parametricRange = new[] { new { Point = Start, Value = GetParametericValue(Start) }, new { Point = End, Value = GetParametericValue(End) } }
                    .OrderBy(x => x.Value)
                    .ToArray();

                var min = parametricRange[0];
                var max = parametricRange[1];

                var otherParametricRange = new[] { new { Point = otherLine.Start, Value = GetParametericValue(otherLine.Start) }, new { Point = otherLine.End, Value = GetParametericValue(otherLine.End) } }
                    .OrderBy(x => x.Value)
                    .ToArray();

                var otherMin = otherParametricRange[0];
                var otherMax = otherParametricRange[1];

                if (min.Value == otherMin.Value)
                {
                    if (max.Value != otherMax.Value)
                    {
                        result = new Line(max.Point, otherMax.Point);
                        return true;
                    }
                }
                else if (max.Value == otherMax.Value)
                {
                    if (min.Value != otherMin.Value)
                    {
                        result = new Line(min.Point, otherMin.Point);
                        return true;
                    }
                }
            }

            result = null;
            return false;
        }

        public PointF GetIntersectionPointWith(Line otherLine)
        {
            var slope = GetSlope();
            var otherSlope = otherLine.GetSlope();

            if (slope == otherSlope)
                throw new NotSupportedException();
            else if (slope == null)
            {
                return new PointF(Start.X, otherLine.GetYValue(Start.X));
            }
            else if (otherSlope == null)
            {
                return new PointF(otherLine.Start.X, GetYValue(otherLine.Start.X));
            }
            else
            {
                var b = GetB();
                var otherB = otherLine.GetB();

                if (b == null || otherB == null)
                    throw new NotSupportedException();

                if (slope.Value != 0 && otherSlope.Value != 0)
                {
                    var intersectionX = (otherB.Value - b.Value) / (slope.Value - otherSlope.Value);
                    var intersectionY = slope.Value * intersectionX + b.Value;// (intersectionY - b.Value) / slope.Value;
                    return new PointF(intersectionX, intersectionY);
                }
                else if (slope.Value == 0)
                {
                    var intersectionY = Start.Y;
                    var intersectionX = (intersectionY - otherB.Value) / otherSlope.Value;
                    return new PointF(intersectionX, intersectionY);
                }
                else
                {
                    var intersectionY = otherLine.Start.Y;
                    var intersectionX = (intersectionY - b.Value) / slope.Value;
                    return new PointF(intersectionX, intersectionY);
                }
            }
        }

        public float GetYValue(float xValue)
        {
            var slope = GetSlope();
            if (slope == null)
                throw new NotSupportedException();

            var xDiff = (xValue - Start.X);
            return Start.Y + slope.Value * xDiff;
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

        public float? GetB()
        {
            var slope = GetSlope();

            if (slope == null)
                return null;

            return Start.Y - slope.Value * Start.X;
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

        public static bool operator ==(Line line1, Line line2)
        {
            if (ReferenceEquals(line1, null))
                return ReferenceEquals(line2, null);

            return line1.Equals(line2);
        }

        public static bool operator !=(Line line1, Line line2)
            => !(line1 == line2);

        public bool Equals(Line other)
        {
            if (ReferenceEquals(other, null))
                return false;

            var points = new[] { Start, End }.OrderBy(x => x.X).ThenBy(x => x.Y);
            var otherPoints = new[] { other.Start, other.End }.OrderBy(x => x.X).ThenBy(x => x.Y);

            return points.SequenceEqual(otherPoints);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Line);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Start.GetHashCode() + End.GetHashCode();
            }
        }
    }

    public class LineD
    {
        public readonly PointD Start;
        public readonly PointD End;

        public PointD[] Points => new[] { Start, End };

        public LineD(PointD start, PointD end)
        {
            Start = start;
            End = end;
        }

        public LineD(PointF start, PointF end)
        {
            Start = new PointD(start);
            End = new PointD(end);
        }

        public PointD GetIntersectionPointWith(LineD otherLine)
        {
            var slope = GetSlope();
            var otherSlope = otherLine.GetSlope();

            if (slope == otherSlope)
                throw new NotSupportedException();
            else if (slope == null)
            {
                return new PointD(Start.X, otherLine.GetYValue(Start.X));
            }
            else if (otherSlope == null)
            {
                return new PointD(otherLine.Start.X, GetYValue(otherLine.Start.X));
            }
            else
            {
                var b = GetB();
                var otherB = otherLine.GetB();

                if (b == null || otherB == null)
                    throw new NotSupportedException();

                if (slope.Value != 0 && otherSlope.Value != 0)
                {
                    var intersectionX = (otherB.Value - b.Value) / (slope.Value - otherSlope.Value);
                    var intersectionY = intersectionX * slope.Value + b.Value;
                    return new PointD(intersectionX, intersectionY);
                }
                else if (slope.Value == 0)
                {
                    var intersectionY = Start.Y;
                    var intersectionX = (intersectionY - otherB.Value) / otherSlope.Value;
                    return new PointD(intersectionX, intersectionY);
                }
                else
                {
                    var intersectionY = otherLine.Start.Y;
                    var intersectionX = (intersectionY - b.Value) / slope.Value;
                    return new PointD(intersectionX, intersectionY);
                }
            }
        }

        public double GetYValue(double xValue)
        {
            var slope = GetSlope();
            if (slope == null)
                throw new NotSupportedException();

            var xDiff = (xValue - Start.X);
            return Start.Y + slope.Value * xDiff;
        }

        public double? GetSlope()
        {
            if (Start.X == End.X)
                return null;
            else
            {
                return (End.Y - Start.Y) / (End.X - Start.X);
            }
        }

        public double? GetB()
        {
            var slope = GetSlope();

            if (slope == null)
                return null;

            return Start.Y - slope.Value * Start.X;
        }

        public double? GetParametericValue(PointD targetPoint)
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
    }

    public struct PointD : IEquatable<PointD>
    {
        public readonly double X;
        public readonly double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointD(PointF pointF)
        {
            X = pointF.X;
            Y = pointF.Y;
        }

        public PointF ToPointF()
            => new PointF((float)X, (float)Y);

        public double GetLength()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }

        public static bool operator !=(PointD point1, PointD point2)
            => !(point1 == point2);

        public static bool operator==(PointD point1, PointD point2)
        {
            if (ReferenceEquals(point1, null))
                return ReferenceEquals(point2, null);

            return point1.Equals(point2);
        }

        public bool Equals(PointD other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is PointD pointD)
                return Equals(pointD);
            else
                return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return X.GetHashCode() + Y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}
