using IllustratorRenderingDescriptions.NavyDigits.How;
using NUnit.Framework;
using System.Drawing;

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
            var script = renderer.CreateScript();
        }
    }
}
