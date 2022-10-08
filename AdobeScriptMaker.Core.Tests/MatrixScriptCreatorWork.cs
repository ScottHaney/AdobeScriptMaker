using AdobeScriptMaker.Core;
using MatrixLayout;
using MatrixLayout.ExpressionDecorators;
using MatrixLayout.ExpressionLayout.LayoutResults;
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
        public void CreateScripts()
        {
            var cakeNutritionScript = CreateCakeNutritionScript();
            var cakeIcingMixtureScript = CreateCakeIcingMixtureScript();
        }

        private string CreateCakeIcingMixtureScript()
        {
            var cakeIcingMixture = new MultiplyComponents(
                new AnnotatedMatrixComponent(
                    new MatrixComponent(new MatrixValuesDescription(3, 4, 408, 455, 194, 78, 0, 0, 50, 1, 0, 13, 0, 6)),
                    new MatrixAnnotations(
                        new List<string>() { "Calories", "Sugar", "Protein" },
                        false,
                        new List<string>() { "Butter", "Flour", "Sugar", "Eggs" },
                        CreateAnnotationsTextSettings(),
                        GetAnnotationsPadding())),
                new AnnotatedMatrixComponent(
                    new MatrixComponent(new MatrixValuesDescription(4, 2, 3, 8, 2, 0, 6, 6, 3, 6)),
                    new MatrixAnnotations(
                        new List<string>() { "Servings of Butter", "Servings of Flour", "Servings of Sugar", "Eggs" },
                        true,
                        new List<string>() { "Cake Recipe", "Icing Recipe"},
                        CreateAnnotationsTextSettings(),
                        GetAnnotationsPadding())));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(cakeIcingMixture);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private string CreateCakeNutritionScript()
        {
            var cakeNutrition = new AnnotatedMatrixComponent(
                new MatrixComponent(new MatrixValuesDescription(3, 2, 3532, 4896, 303, 306, 44, 36)),
                new MatrixAnnotations(
                    new List<string>() { "Calories", "Sugar", "Protein" },
                    true,
                    new List<string>() { "Cake Recipe", "Icing Recipe" },
                    CreateAnnotationsTextSettings(),
                    GetAnnotationsPadding()));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(cakeNutrition);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private TextSettings CreateAnnotationsTextSettings()
        {
            return new TextSettings(new System.Drawing.Font("Arial", 55, System.Drawing.GraphicsUnit.Pixel));
        }

        private int GetAnnotationsPadding()
        {
            return 37;
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
