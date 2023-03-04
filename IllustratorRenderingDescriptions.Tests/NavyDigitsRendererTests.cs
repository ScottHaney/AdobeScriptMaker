using IllustratorRenderingDescriptions.NavyDigits;
using IllustratorRenderingDescriptions.NavyDigits.How;
using IllustratorRenderingDescriptions.NavyDigits.How.ChiselActions;
using NUnit.Framework;
using System;
using System.Drawing;
using System.Linq;

namespace IllustratorRenderingDescriptions.Tests
{
    public class NavyDigitsRendererTests
    {
        [Test]
        public void Create_Digits()
        {
            var heightToWidthRatio = 1.5f;
            var width = 300;
            var height = heightToWidthRatio * width;

            var renderer = new NavyDigitsRenderer(new SizeF(width, height));
            var script = renderer.CreateScript(Enumerable.Range(0, 10).ToArray());
        }

        [Test]
        public void Create_Items_For_Programming_Art_Video()
        {
            var heightToWidthRatio = 1.5f;
            var width = 300;
            var height = heightToWidthRatio * width;

            var widthPaddingPercentage = 0.25f;
            var triangleInsetPaddingPercentage = 0.5f * widthPaddingPercentage;
            var holeWidthPaddingPercentage = 0.20f;
            var overhangPercentage = 0.4f;
            var shadowWidthPercentage = 1 / 8.0f;

            var strokeWidth = 0;
            var boundingBoxForBlock = new RectangleF(new PointF(0, 0), new SizeF(width * 2, height));

            var sculpture = new DigitSculpture(boundingBoxForBlock)
                { Id = "block", StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

            var script1 = sculpture.Carve();

            var renderer = new NavyDigitsRenderer(new SizeF(width, height));
            var script2 = renderer.CreateScript(0, 1);

            var finalScript = string.Join(Environment.NewLine, script1, script2);
        }


        [Test]
        public void Create_Hull_Numbers()
        {
            var heightToWidthRatio = 1.5f;
            var width = 300;
            var height = heightToWidthRatio * width;

            var widthPaddingPercentage = 0.25f;
            var triangleInsetPaddingPercentage = 0.5f * widthPaddingPercentage;
            var holeWidthPaddingPercentage = 0.20f;
            var overhangPercentage = 0.4f;
            var shadowWidthPercentage = 1 / 8.0f;

            var strokeWidth = 0;
            var boundingBoxForBlock = new RectangleF(new PointF(0, 0), new SizeF(width * 2, height));

            var sculpture = new DigitSculpture(boundingBoxForBlock)
            { Id = "block", StrokeWidth = strokeWidth, ShadowWidthPercentage = shadowWidthPercentage };

            var script1 = sculpture.Carve();

            var renderer = new NavyDigitsRenderer(new SizeF(width, height));
            var script2 = renderer.CreateScript(0, 1);

            var finalScript = string.Join(Environment.NewLine, script1, script2);
        }
    }
}
