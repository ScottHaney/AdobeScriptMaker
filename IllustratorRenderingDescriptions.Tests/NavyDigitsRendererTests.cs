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
            var renderer = new NavyDigitsRenderer(new SizeF(250, 400));
            var script = renderer.CreateScript();
        }
    }
}
