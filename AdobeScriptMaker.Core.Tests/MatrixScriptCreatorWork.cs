using AdobeScriptMaker.Core;
using MatrixLayout;
using MatrixLayout.ExpressionDecorators;
using MatrixLayout.InputDescriptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdobeScriptMaker.Core.Tests
{
    public class MatrixScriptCreatorWork
    {
        [Test]
        public void CreateMatrixVideoScripts()
        {
            var cakeNutritionScript = CreateCakeNutritionScript();
        }

        private string CreateCakeNutritionScript()
        {
            var cakeNutrition = new AnnotatedMatrixComponent(
                new MatrixComponent(new MatrixValuesDescription(3, 2, 3532, 4896, 303, 306, 44, 36)),
                new MatrixAnnotations(
                    new List<string>() { "Calories", "Sugar", "Protein" },
                    true,
                    new List<string>() { "Cake Recipe", "Icing Recipe" },
                    new MatrixLayout.ExpressionLayout.LayoutResults.TextSettings(new System.Drawing.Font("Arial", 72, System.Drawing.GraphicsUnit.Pixel)),
                    37));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(cakeNutrition);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private ExpressionManager CreateExpressionManager()
        {
            var textSettings = new TextDisplayDescription("Arial", 72);
            var matrixLayoutSettings = new MatrixLayoutDescription(
                    new MatrixBracketsDescription(3, 35),
                    new MatrixInteriorMarginsDescription(0.5f, 0.1f, 0.5f, 1),
                    12, 20, 35);

            return new ExpressionManager(new ExpressionDisplaySettings(
                textSettings,
                matrixLayoutSettings));
        }
    }
}
