using MatrixLayout.InputDescriptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public List<PointF> GetRightBracketPathPoints()
        {
            var pointsCreator = new PointsCreator(new PointF(Bounds.Right, Bounds.Top));

            pointsCreator.MoveLeft(BracketsSettings.Thickness + BracketsSettings.PincerWidth);
            pointsCreator.MoveDown(BracketsSettings.Thickness);

            pointsCreator.MoveRight(BracketsSettings.PincerWidth);
            pointsCreator.MoveDown(Bounds.Height - 2 * BracketsSettings.Thickness);

            pointsCreator.MoveLeft(BracketsSettings.PincerWidth);
            pointsCreator.MoveDown(BracketsSettings.Thickness);

            pointsCreator.MoveRight(BracketsSettings.Thickness + BracketsSettings.PincerWidth);

            return pointsCreator.Results;
        }

        public void ShiftDown(float shift)
        {
            Bounds = new RectangleF(Bounds.Left,
                Bounds.Top + shift,
                Bounds.Width,
                Bounds.Height);
        }

        private class PointsCreator
        {
            private readonly List<PointF> _points = new List<PointF>();

            public List<PointF> Results => _points;

            public PointsCreator(PointF startingPoint)
            {
                _points.Add(startingPoint);
            }

            public void MoveLeft(float left)
                => MoveX(-left);

            public void MoveRight(float right)
                => MoveX(right);

            public void MoveUp(float up)
                => MoveY(-up);

            public void MoveDown(float down)
                => MoveY(down);

            private void MoveX(float distance)
            {
                _points.Add(new PointF(_points.Last().X + distance, _points.Last().Y));
            }

            private void MoveY(float distance)
            {
                _points.Add(new PointF(_points.Last().X, _points.Last().Y + distance));
            }
        }
    }
}
