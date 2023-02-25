using Geometry.Lines;
using Geometry.LineSegments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Geometry
{
    public class LineDivider2
    {
        public IEnumerable<LineSegment> DivideLines(LineSegment targetLineSegment, IEnumerable<LineSegment> lineSegementsToDivide)
        {
            var results = new List<LineSegment>() { targetLineSegment };
            foreach (var item in lineSegementsToDivide)
            {
                if (!results.Any())
                    break;

                results = results.SelectMany(x => DivideLine(x, item)).ToList();
            }

            return results;
        }

        public List<LineSegment> DivideLine(LineSegment targetLineSegment, LineSegment lineSegmentToDivideWith)
        {
            var targetLine = targetLineSegment.ToLine();
            var lineToDivideWith = lineSegmentToDivideWith.ToLine();

            var intersectionResult = targetLine.GetIntersectionWith(lineToDivideWith);

            if (intersectionResult.IsTheSameLine)
                return GetSameLineResults(targetLineSegment, lineSegmentToDivideWith);
            else
                return new List<LineSegment>() { targetLineSegment };
        }

        private List<LineSegment> GetSameLineResults(LineSegment targetLineSegment, LineSegment lineSegmentToDivideWith)
        {
            var targetLine = targetLineSegment.ToLine();

            var targetValues = targetLineSegment.GetParametricRange();
            var divideWithValues = lineSegmentToDivideWith.GetParametricRange();
            
            var results = new List<LineSegment>();
            if (divideWithValues.End.ParametricValue <= targetValues.Start.ParametricValue)
                results.Add(targetLineSegment);
            else if (divideWithValues.End.ParametricValue > targetValues.Start.ParametricValue && divideWithValues.End.ParametricValue <= targetValues.End.ParametricValue)
            {
                results.AddRange(CreateLineSegment(targetLine, divideWithValues.End, targetValues.End));
                if (divideWithValues.Start.ParametricValue > targetValues.Start.ParametricValue)
                    results.AddRange(CreateLineSegment(targetLine, targetValues.Start, divideWithValues.Start));
            }
            else
            {
                if (divideWithValues.Start.ParametricValue < targetValues.End.ParametricValue)
                    results.AddRange(CreateLineSegment(targetLine, targetValues.Start, divideWithValues.Start));
                else
                    results.Add(targetLineSegment);
            }

            return results;
        }

        private IEnumerable<LineSegment> CreateLineSegment(ILineRepresentation line, ParametricPoint p1, ParametricPoint p2)
        {
            //Make sure to avoid adding any empty line segments
            if (p1.ParametricValue != p2.ParametricValue)
                yield return new LineSegment(line, p1.Point, p2.Point);
        }
    }

    public class LineDivider
    {
        public IEnumerable<Line> DivideLine(Line targetLine, Line lineToDivideWith)
        {
            var internalResults = DivideLineInternal(targetLine, lineToDivideWith);
            return internalResults.Where(x => x.Start != x.End);
        }

        private IEnumerable<Line> DivideLineInternal(Line targetLine, Line lineToDivideWith)
        {
            //Make sure that both lines are oriented in the same parametric direction For example the line [(0,0), (1,1)] and the line [(1,1), (0,0)] should have
            //the same return value from this method
            if (targetLine.GetSlope() == null && lineToDivideWith.GetSlope() == null)
            {
                //Vertical lines, sort by y values
                if (targetLine.Start.Y > targetLine.End.Y)
                    targetLine = new Line(targetLine.End, targetLine.Start);
                if (lineToDivideWith.Start.Y > lineToDivideWith.End.Y)
                    lineToDivideWith = new Line(lineToDivideWith.End, lineToDivideWith.Start);
            }
            else
            {
                //Non vertical lines, sort by x values
                if (targetLine.Start.X > targetLine.End.X)
                    targetLine = new Line(targetLine.End, targetLine.Start);
                if (lineToDivideWith.Start.X > lineToDivideWith.End.X)
                    lineToDivideWith = new Line(lineToDivideWith.End, lineToDivideWith.Start);
            }

            var parametricDivideStart = targetLine.GetParametericValue(lineToDivideWith.Start);
            var parametricDivideEnd = targetLine.GetParametericValue(lineToDivideWith.End);

            if (parametricDivideStart == null || parametricDivideEnd == null)
            {
                yield return targetLine;
                yield break;
            }

            var parametricTargetStart = targetLine.GetParametericValue(targetLine.Start);
            var parametricTargetEnd = targetLine.GetParametericValue(targetLine.End);

            //The divide line and the target line are the same line in this case just return empty
            if (parametricTargetStart.Value == parametricDivideStart.Value && parametricTargetEnd.Value == parametricDivideEnd.Value)
                yield break;

            if (parametricDivideEnd.Value < parametricTargetStart.Value)
                yield return targetLine;
            else if (parametricDivideEnd.Value < parametricTargetEnd.Value)
            {
                if (parametricDivideStart.Value < parametricTargetStart.Value)
                    yield return new Line(lineToDivideWith.End, targetLine.End);
                else
                {
                    yield return new Line(targetLine.Start, lineToDivideWith.Start);
                    yield return new Line(lineToDivideWith.End, targetLine.End);
                }
            }
            else
            {
                if (parametricDivideStart.Value > parametricTargetStart.Value)
                {
                    if (parametricDivideStart.Value > parametricTargetEnd.Value)
                        yield return targetLine;
                    else
                        yield return new Line(targetLine.Start, lineToDivideWith.Start);
                }
                else
                    yield break;
            } 
        }

    }
}
