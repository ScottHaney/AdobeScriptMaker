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
            var line1 = new LineD(new PointD(0, 1), new PointD(1, 2));
            var line2 = new LineD(new PointD(0, 1), new PointD(-1, 2));

            var intersectionPoint = line1.GetIntersectionPointWith(line2);

            Assert.AreEqual(0, intersectionPoint.X);
            Assert.AreEqual(1, intersectionPoint.Y);
        }
    }
}