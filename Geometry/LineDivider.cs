using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Geometry
{
    public class LineDivider
    {
        public IEnumerable<Line> DivideLine(Line targetLine, Line lineToDivideWith)
        {
            var parametricDivideStart = targetLine.GetParametericValue(lineToDivideWith.Start);
            var parametricDivideEnd = targetLine.GetParametericValue(lineToDivideWith.End);

            if (parametricDivideStart == null || parametricDivideEnd == null)
                yield return targetLine;

            var parametricTargetStart = targetLine.GetParametericValue(targetLine.Start);
            var parametricTargetEnd = targetLine.GetParametericValue(targetLine.End);

            if (parametricDivideEnd.Value < parametricTargetStart.Value)
                yield return targetLine;
            else if (parametricDivideEnd.Value < parametricTargetEnd.Value)
            {
                if (parametricDivideStart.Value < parametricTargetStart.Value)
                    yield return new Line(targetLine.Start, lineToDivideWith.End);
                else
                {
                    yield return new Line(targetLine.Start, lineToDivideWith.Start);
                    yield return new Line(lineToDivideWith.End, targetLine.End);
                }
            }
            else
            {
                if (parametricDivideStart.Value < parametricTargetStart.Value)
                    yield return new Line(targetLine.Start, lineToDivideWith.Start);
                else
                    yield return targetLine;
            } 
        }

    }
}
