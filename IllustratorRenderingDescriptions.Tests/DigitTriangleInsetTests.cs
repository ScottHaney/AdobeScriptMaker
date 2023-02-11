using IllustratorRenderingDescriptions.NavyDigits.How;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IllustratorRenderingDescriptions.Tests
{
    public class DigitTriangleInsetTests
    {
        [Test]
        public void Creates_The_Left_Triangle_Inset()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var inset = new DigitTriangleInset(DigitTriangleInsetName.Left, 0.1f, 45f);

            var actualResults = inset.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(0, 60), new PointF(10, 50), new PointF(0, 40) }, actualResults);
        }

        [Test]
        public void Creates_The_Right_Triangle_Inset()
        {
            var digitBoundingBox = new RectangleF(0, 0, 100, 100);
            var inset = new DigitTriangleInset(DigitTriangleInsetName.Right, 0.1f, 45f);

            var actualResults = inset.GetPoints(digitBoundingBox);

            CollectionAssert.AreEqual(new[] { new PointF(100, 40), new PointF(90, 50), new PointF(100, 60) }, actualResults);
        }
    }
}
