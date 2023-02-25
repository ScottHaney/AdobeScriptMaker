using Geometry.Lines;
using Geometry.LineSegments;
using NUnit.Framework;
using System.Drawing;

namespace Geometry.Tests
{
    public class LineTests
    {
        [Test]
        public void Slope_Of_A_Vertical_Line_Is_Null()
        {
            var line = new Line(new PointF(0, 1), new PointF(0, 10));

            Assert.AreEqual(null, line.GetSlope());
        }

        [Test]
        public void Slope_Of_A_45_Degree_Line_From_Left_To_Right_Is_One()
        {
            var line = new Line(new PointF(0, 0), new PointF(1, 1));

            Assert.AreEqual(1, line.GetSlope());
        }

        [Test]
        public void Slope_Of_A_45_Degree_Line_From_Right_To_Left_Is_One()
        {
            var line = new Line(new PointF(1, 1), new PointF(0, 0));

            Assert.AreEqual(1, line.GetSlope());
        }

        [Test]
        public void The_Parametric_Value_Of_A_Point_Not_On_A_Non_Vertical_Line_Is_Null()
        {
            var line = new Line(new PointF(0, 0), new PointF(1, 1));
            var actualResult = line.GetParametericValue(new PointF(0, 5));

            Assert.AreEqual(null, actualResult);
        }

        [Test]
        public void The_Parametric_Value_Of_A_Point_On_A_Non_Vertical_Line_To_The_Right_Of_The_Start_Point_Is_The_X_Difference_From_The_Start_Point()
        {
            var line = new Line(new PointF(0, 0), new PointF(1, 1));
            var actualResult = line.GetParametericValue(new PointF(2, 2));

            Assert.AreEqual(2, actualResult);
        }

        [Test]
        public void The_Parametric_Value_Of_A_Point_On_A_Non_Vertical_Line_To_The_Left_Of_The_Start_Point_Is_The_X_Difference_From_The_Start_Point()
        {
            var line = new Line(new PointF(0, 0), new PointF(1, 1));
            var actualResult = line.GetParametericValue(new PointF(-1, -1));

            Assert.AreEqual(-1, actualResult);
        }

        [Test]
        public void The_Parametric_Value_Of_A_Point_Not_On_A_Vertical_Line_Is_Null()
        {
            var line = new Line(new PointF(0, 0), new PointF(0, 1));
            var actualResult = line.GetParametericValue(new PointF(5, 5));

            Assert.AreEqual(null, actualResult);
        }

        [Test]
        public void The_Parametric_Value_Of_A_Point_On_A_Vertical_Line_Above_The_Start_Point_Is_The_Y_Difference_From_The_Start_Point()
        {
            var line = new Line(new PointF(0, 0), new PointF(0, 1));
            var actualResult = line.GetParametericValue(new PointF(0, 2));

            Assert.AreEqual(2, actualResult);
        }

        [Test]
        public void The_Parametric_Value_Of_A_Point_On_A_Vertical_Line_Below_The_Start_Point_Is_The_Y_Difference_From_The_Start_Point()
        {
            var line = new Line(new PointF(0, 0), new PointF(0, 1));
            var actualResult = line.GetParametericValue(new PointF(0, -1));

            Assert.AreEqual(-1, actualResult);
        }

        [Test]
        public void The_Intersection_Of_Two_Perpendicular_Lines_Is_Calculated_Correctly()
        {
            var line1 = new LineD(new PointD(-1, -1), new PointD(1, 1));
            var line2 = new LineD(new PointD(-1, 1), new PointD(1, -1));

            var intersectionPoint = line1.GetIntersectionPointWith(line2);

            Assert.AreEqual(0, intersectionPoint.X);
            Assert.AreEqual(0, intersectionPoint.Y);
        }

        [Test]
        public void Two_Identical_Horizontal_Lines_Are_Equal()
        {
            var factory = new LineRepresentationFactory();

            TestLinesEquality(
                factory.CreateLine(new PointD(0, 5), new PointD(10, 5)),
                factory.CreateLine(new PointD(0, 5), new PointD(10, 5)));
        }

        [Test]
        public void Two_Identical_Vertical_Lines_Are_Equal()
        {
            var factory = new LineRepresentationFactory();

            TestLinesEquality(
                factory.CreateLine(new PointD(1, 5), new PointD(1, 15)),
                factory.CreateLine(new PointD(1, 5), new PointD(1, 15)));
        }

        [Test]
        public void Two_45_Degree_Lines_With_Different_Intervals_Are_Equal()
        {
            var factory = new LineRepresentationFactory();

            TestLinesEquality(
                factory.CreateLine(new PointD(1, 1), new PointD(10, 10)),
                factory.CreateLine(new PointD(20, 20), new PointD(30, 30)));
        }

        private void TestLinesEquality(LineRepresentation rep1, LineRepresentation rep2)
        {
            Assert.IsTrue(rep1 == rep2);
            Assert.IsTrue(rep2 == rep1);
            Assert.IsFalse(rep1 != rep2);
            Assert.IsFalse(rep2 != rep1);

            Assert.IsTrue(rep1.Equals(rep2));
            Assert.IsTrue(rep2.Equals(rep1));
        }
    }
}