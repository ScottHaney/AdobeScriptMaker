using AdobeScriptMaker.Core;
using MatrixLayout;
using MatrixLayout.ExpressionDecorators;
using MatrixLayout.InputDescriptions;
using NUnit.Framework;

namespace Applications.Tests
{
    public class Tests
    {
        [Test]
        public void Test()
        {
            var expressionManager = new ExpressionManager(new ExpressionDisplaySettings(
                new TextDisplayDescription("Arial", 72),
                new MatrixLayoutDescription(
                    new MatrixBracketsDescription(2, 35),
                    new MatrixInteriorMarginsDescription(0, 0, 0))));

            var expression = new MatrixComponent(new MatrixValuesDescription(2, 2, 1, 1, 1, 1));

            var result = expressionManager.Render(expression);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(result);
        }
    }
}