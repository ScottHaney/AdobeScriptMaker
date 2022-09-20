using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.Tests
{
    public class TextMeasurerTests
    {
        [Test]
        public void RunsWithoutError()
        {
            var textMeasurer = new TextMeasurer();
            textMeasurer.MeasureText("Testing", new System.Drawing.Font("Arial", 12));
        }
    }
}
