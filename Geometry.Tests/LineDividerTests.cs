using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Geometry.Tests
{
    public class LineDividerTests
    {
        [Test]
        public void Dividing_By_A_Non_Parallel_Line_Returns_The_Original_Line()
        {
            var divider = new LineDivider();

            var targetLine = new Line(new PointF(0, 0), new PointF(1, 0));
            var divideLine = new Line(new PointF(0, 0), new PointF(0, 1));

            var actualResult = divider.DivideLine(targetLine, divideLine);
            CollectionAssert.AreEqual(new[] { targetLine }, actualResult);
        }

        [Test]
        public void Dividing_By_The_Same_Line_Returns_Empty()
        {
            var divider = new LineDivider();

            var targetLine = new Line(new PointF(0, 0), new PointF(1, 1));
            var divideLine = new Line(new PointF(1, 1), new PointF(0, 0));

            var actualResult = divider.DivideLine(targetLine, divideLine);
            CollectionAssert.AreEqual(Array.Empty<Line>(), actualResult);
        }

        [Test]
        public void Dividing_By_A_Line_That_Could_Match_But_Ends_Before_The_Start_Of_The_Line_Returns_The_Original_Line()
        {
            var divider = new LineDivider();

            var targetLine = new Line(new PointF(0, 0), new PointF(1, 1));
            var divideLine = new Line(new PointF(-2, -2), new PointF(-1, -1));

            var actualResult = divider.DivideLine(targetLine, divideLine);
            CollectionAssert.AreEqual(new[] { targetLine }, actualResult);
        }

        [Test]
        public void Dividing_By_A_Line_That_Matches_Starts_Before_The_Start_Of_The_Line_And_Ends_Before_The_End_Of_The_Line_Creates_A_New_Line()
        {
            var divider = new LineDivider();

            var targetLine = new Line(new PointF(0, 0), new PointF(1, 1));
            var divideLine = new Line(new PointF(-2, -2), new PointF(0.5f, 0.5f));

            var actualResult = divider.DivideLine(targetLine, divideLine);
            CollectionAssert.AreEqual(new[] {new Line(new PointF(0.5f, 0.5f), new PointF(1, 1))}, actualResult);
        }

        [Test]
        public void Dividing_By_A_Line_That_Matches_Starts_After_The_Start_Of_The_Line_And_Ends_Before_The_End_Of_The_Line_Creates_Two_New_Lines()
        {
            var divider = new LineDivider();

            var targetLine = new Line(new PointF(0, 0), new PointF(1, 1));
            var divideLine = new Line(new PointF(0.25f, 0.25f), new PointF(0.5f, 0.5f));

            var actualResult = divider.DivideLine(targetLine, divideLine);
            CollectionAssert.AreEqual(new[] { new Line(new PointF(0, 0), new PointF(0.25f, 0.25f)), new Line(new PointF(0.5f, 0.5f), new PointF(1, 1)) }, actualResult);
        }

        [Test]
        public void Dividing_By_A_Line_That_Matches_Starts_After_The_Start_Of_The_Line_And_Ends_After_The_End_Of_The_Line_Creates_A_New_Line()
        {
            var divider = new LineDivider();

            var targetLine = new Line(new PointF(0, 0), new PointF(1, 1));
            var divideLine = new Line(new PointF(0.5f, 0.5f), new PointF(2, 2));

            var actualResult = divider.DivideLine(targetLine, divideLine);
            CollectionAssert.AreEqual(new[] { new Line(new PointF(0, 0), new PointF(0.5f, 0.5f)) }, actualResult);
        }

        [Test]
        public void Dividing_By_A_Line_That_Could_Match_But_Starts_After_The_End_Of_The_Line_Returns_The_Original_Line()
        {
            var divider = new LineDivider();

            var targetLine = new Line(new PointF(0, 0), new PointF(1, 1));
            var divideLine = new Line(new PointF(2, 2), new PointF(3, 3));

            var actualResult = divider.DivideLine(targetLine, divideLine);
            CollectionAssert.AreEqual(new[] { targetLine }, actualResult);
        }

        [Test]
        public void Dividing_By_A_Line_That_Contains_The_Target_Line_Returns_Empty()
        {
            var divider = new LineDivider();

            var targetLine = new Line(new PointF(0, 0), new PointF(1, 1));
            var divideLine = new Line(new PointF(-1, -1), new PointF(3, 3));

            var actualResult = divider.DivideLine(targetLine, divideLine);
            CollectionAssert.AreEqual(Array.Empty<Line>(), actualResult);
        }
    }
}
