using MathRenderingDescriptions.Plot;
using MathRenderingDescriptions.Plot.What;
using MathRenderingDescriptions.Plot.What.RiemannSums;
using NUnit.Framework;
using System.Drawing;

namespace MathRenderingDescriptions.Tests
{
    public class RiemannSumsMetadataTests
    {
        [Test]
        public void Finds_Area_Of_Single_RiemannSum_For_YEqualsX()
        {
            var plotLayoutDescription = new PlotLayoutDescription(
                new PlotAxesLayoutDescription(
                    new PlotAxisLayoutDescription(800, 0, 5),
                    new PlotAxisLayoutDescription(800, 0, 5)), new PointF(100, 100));

            var function = new FunctionRenderingDescription("FuncDesc",
                plotLayoutDescription,
                x => x);

            var sumsProvider = new SumsProvider(1);
            var riemannSums = new RiemannSumsRenderingDescription("SumsDesc",
                function,
                new FitToDuration(sumsProvider),
                sumsProvider);

            var metadata = riemannSums.GetMetadata();

            Assert.AreEqual(1, metadata.SumsDetails.Length);
            Assert.AreEqual(1, metadata.SumsDetails[0].NumSums);
            Assert.AreEqual(25, metadata.SumsDetails[0].TotalArea);
        }
    }
}