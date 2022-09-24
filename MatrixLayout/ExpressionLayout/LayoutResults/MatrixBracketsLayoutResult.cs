using MatrixLayout.InputDescriptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class MatrixBracketsLayoutResult : ILayoutResult
    {
        public RectangleF Bounds { get; private set; }
        public readonly MatrixBracketsDescription BracketsSettings;

        public MatrixBracketsLayoutResult(RectangleF outerBounds,
            MatrixBracketsDescription bracketsSettings)
        {
            Bounds = outerBounds;
            BracketsSettings = bracketsSettings;
        }

        public List<PointF> GetLeftBracketPathPoints()
        {
            var points = new List<PointF>();

            points.Add(new PointF(Bounds.Left + BracketsSettings.Thickness + BracketsSettings.PincerWidth,
                Bounds.Top));

            points.Add(new PointF(Bounds.Left, Bounds.Top));
            points.Add(new PointF(Bounds.Left, Bounds.Bottom));

            points.Add(new PointF(Bounds.Left + BracketsSettings.Thickness + BracketsSettings.PincerWidth,
                Bounds.Bottom));

            points.Add(new PointF(Bounds.Left + BracketsSettings.Thickness + BracketsSettings.PincerWidth,
                            Bounds.Bottom - BracketsSettings.Thickness));

            points.Add(new PointF(Bounds.Left + BracketsSettings.Thickness,
                Bounds.Bottom - BracketsSettings.Thickness));

            points.Add(new PointF(Bounds.Left + BracketsSettings.Thickness,
                Bounds.Top + BracketsSettings.Thickness));

            points.Add(new PointF(Bounds.Left + BracketsSettings.Thickness + BracketsSettings.PincerWidth,
                Bounds.Top + BracketsSettings.Thickness));

            return points;
        }
    }
}
