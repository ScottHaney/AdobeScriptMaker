using AdobeScriptMaker.Core;
using MatrixLayout;
using MatrixLayout.ExpressionDecorators;
using MatrixLayout.ExpressionLayout.LayoutResults;
using MatrixLayout.InputDescriptions;
using NUnit.Framework;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdobeScriptMaker.Core.Tests
{
    public class MatrixScriptCreatorWork
    {
        [Test]
        public void CreateScripts_MatricesATastyIntroduction()
        {
            var cakeNutritionScript = CreateCakeNutritionScript();
            var cakeIcingMixtureScript = CreateCakeIcingMixtureScript();
            var icingVectorSumScript = CreateIcingVectorSumScript();
            var cakeVectorSumScript = CreateCakeVectorSumScript();
            var totalNutritionSumScript = CreateTotalNutritionSumScript();
        }

        [Test]
        public void CreateScripts_MatricesAsComponentsShort()
        {
            var snackBarNutrition = CreateSnackBarScript();
            var sandwhichNutrition = CreatePeanutButterSandwhichScript();
        }

        [Test]
        public void CreateScripts_MatrixMultiplicationInRealLife()
        {
            var ingredients = CreateIngredientsScript();
            var ingedientsTimeServingsExpressionScript = CreateIngredientsTimesServingsExpressionScript();
            var ingedientsTimeServingsEquationScript = CreateIngredientsTimesServingsEquationScript();

            var pancakesVectorComboScript = CreatePancakesVectorComboScript();
            var glassOfMilkVectorComboScript = CreateGlassOfMilkVectorComboScript();

            var entireBreakfastScript = CreateEntireBreakfastScript();
        }

        private string CreatePancakesVectorComboScript()
        {
            var expression = new Equation(
                AddComponents.Create(
                    new NumericMultiplierComponent(1,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 120, 12, 8),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Milk" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                    new NumericMultiplierComponent(1,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 70, 0, 6),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Egg" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                    new NumericMultiplierComponent(5,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 190, 8, 14),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Pancake Mix" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding())))),
                new AnnotatedMatrixComponent(
                    new MatrixComponent(3, 1, 1140, 52, 84),
                    new MatrixAnnotations(
                        new List<string>() { "Calories", "Sugar", "Protein" },
                        true,
                        new List<string>() { "Pancakes Nutrition" },
                        CreateAnnotationsTextSettings(),
                        GetAnnotationsPadding())));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(expression);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private string CreateGlassOfMilkVectorComboScript()
        {
            var expression = new Equation(
                AddComponents.Create(
                    new NumericMultiplierComponent(2,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 120, 12, 8),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Milk" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                    new NumericMultiplierComponent(0,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 70, 0, 6),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Egg" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                    new NumericMultiplierComponent(0,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 190, 8, 14),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Pancake Mix" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding())))),
                new AnnotatedMatrixComponent(
                    new MatrixComponent(3, 1, 240, 24, 16),
                    new MatrixAnnotations(
                        new List<string>() { "Calories", "Sugar", "Protein" },
                        true,
                        new List<string>() { "Glass of Milk Nutrition" },
                        CreateAnnotationsTextSettings(),
                        GetAnnotationsPadding())));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(expression);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private string CreateIngredientsScript()
        {
            var eggNutrition = new AnnotatedMatrixComponent(
                new MatrixComponent(new MatrixValuesDescription(3, 1, 70, 0, 6)),
                new MatrixAnnotations(
                    new List<string>() { "Calories", "Sugar", "Protein" },
                    true,
                    new List<string>() { "Egg" },
                    CreateAnnotationsTextSettings(),
                    GetAnnotationsPadding()));

            var milkNutrition = new AnnotatedMatrixComponent(
                new MatrixComponent(new MatrixValuesDescription(3, 1, 120, 12, 8)),
                new MatrixAnnotations(
                    new List<string>() { "Calories", "Sugar", "Protein" },
                    true,
                    new List<string>() { "Milk" },
                    CreateAnnotationsTextSettings(),
                    GetAnnotationsPadding()));

            var mixNutrition = new AnnotatedMatrixComponent(
                new MatrixComponent(new MatrixValuesDescription(3, 1, 190, 8, 14)),
                new MatrixAnnotations(
                    new List<string>() { "Calories", "Sugar", "Protein" },
                    true,
                    new List<string>() { "Pancake Mix" },
                    CreateAnnotationsTextSettings(),
                    GetAnnotationsPadding()));

            var expressionManager = CreateExpressionManager();
            var layoutResults = new[] { milkNutrition, eggNutrition, mixNutrition }.Select(x => expressionManager.Render(x));

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults.ToArray());

            return script;
        }

        private string CreateIngredientsTimesServingsExpressionScript()
        {
            var expression = new MultiplyComponents(
                    new AnnotatedMatrixComponent(
                        new MatrixComponent(new MatrixValuesDescription(3, 3, 120, 70, 190, 12, 0, 8, 8, 6, 14)),
                        new MatrixAnnotations(
                            new List<string>() { "Calories", "Sugar", "Protein" },
                            false,
                            new List<string>() { "Milk", "Egg", "Pancake Mix" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding())),
                    new AnnotatedMatrixComponent(
                        new MatrixComponent(new MatrixValuesDescription(3, 2, 1, 2, 1, 0, 5, 0)),
                        new MatrixAnnotations(
                            new List<string>() { "Servings of Milk", "Eggs Used", "Servings of Pancake Mix" },
                            true,
                            new List<string>() { "Pancake Ingredients", "Glass of Milk Ingredients" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding())));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(expression);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private string CreateIngredientsTimesServingsEquationScript()
        {
            var expression = new Equation(
                new MultiplyComponents(
                    new AnnotatedMatrixComponent(
                        new MatrixComponent(new MatrixValuesDescription(3, 3, 120, 70, 190, 12, 0, 8, 8, 6, 14)),
                        new MatrixAnnotations(
                            new List<string>() { "Calories", "Sugar", "Protein" },
                            false,
                            new List<string>() { "Egg", "Milk", "Pancake Mix" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding())),
                    new AnnotatedMatrixComponent(
                        new MatrixComponent(new MatrixValuesDescription(3, 2, 1, 2, 1, 0, 5, 0)),
                        new MatrixAnnotations(
                            new List<string>() { "Eggs Used", "Servings of Milk", "Servings of Pancake Mix" },
                            true,
                            new List<string>() { "Pancake Ingredients", "Glass of Milk Ingredients" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                new AnnotatedMatrixComponent(
                        new MatrixComponent(new MatrixValuesDescription(3, 2, 1140, 240, 52, 24, 84, 16)),
                        new MatrixAnnotations(
                            new List<string>() { "Calories", "Sugar", "Protein" },
                            true,
                            new List<string>() { "Pancakes Nutrition", "Glass of Milk Nutrition" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding())));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(expression);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private string CreateEntireBreakfastScript()
        {
            var expression = new Equation(
                new MultiplyComponents(
                    new AnnotatedMatrixComponent(
                        new MatrixComponent(new MatrixValuesDescription(3, 2, 1140, 240, 52, 24, 84, 16)),
                        new MatrixAnnotations(
                            new List<string>() { "Calories", "Sugar", "Protein" },
                            true,
                            new List<string>() { "Pancakes", "Glass of Milk" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding())),
                    new MatrixComponent(new MatrixValuesDescription(2, 1, 1, 1))),
                new AnnotatedMatrixComponent(
                new MatrixComponent(new MatrixValuesDescription(3, 1, 1380, 76, 100)),
                new MatrixAnnotations(
                    new List<string>() { "Calories", "Sugar", "Protein" },
                    true,
                    new List<string>() { "Entire Breakfast Nutrition" },
                    CreateAnnotationsTextSettings(),
                    GetAnnotationsPadding())));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(expression);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private string CreatePeanutButterSandwhichScript()
        {
            var sandwhichNutrition = new AnnotatedMatrixComponent(
                new MatrixComponent(new MatrixValuesDescription(3, 3, 190, 3, 7, 120, 4, 6, 60, 17, 0)),
                new MatrixAnnotations(
                    new List<string>() { "Calories", "Sugar", "Protein" },
                    true,
                    new List<string>() { "Peanut Butter", "Bread", "Honey" },
                    CreateAnnotationsTextSettings(),
                    GetAnnotationsPadding()));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(sandwhichNutrition);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private string CreateSnackBarScript()
        {
            var snackBarNutrition = new AnnotatedMatrixComponent(
                new MatrixComponent(new MatrixValuesDescription(3, 1, 180, 5, 6)),
                new MatrixAnnotations(
                    new List<string>() { "Calories", "Sugar", "Protein" },
                    true,
                    null,
                    CreateAnnotationsTextSettings(),
                    GetAnnotationsPadding()));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(snackBarNutrition);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private string CreateTotalNutritionSumScript()
        {
            var totalNutritionSum = new Equation(
                new MultiplyComponents(
                    new AnnotatedMatrixComponent(
                    new MatrixComponent(new MatrixValuesDescription(3, 2, 3532, 4896, 303, 306, 44, 36)),
                    new MatrixAnnotations(
                        new List<string>() { "Calories", "Sugar", "Protein" },
                        false,
                        new List<string>() { "Cake Recipe", "Icing Recipe" },
                        CreateAnnotationsTextSettings(),
                        GetAnnotationsPadding())),
                    new MatrixComponent(new MatrixValuesDescription(2, 1, 1, 1))),
                new AnnotatedMatrixComponent(
                    new MatrixComponent(new MatrixValuesDescription(3, 1, 8428, 609, 80)),
                    new MatrixAnnotations(
                        new List<string>() { "Calories", "Sugar", "Protein" },
                        true,
                        new List<string>() { "Entire Cake" },
                        CreateAnnotationsTextSettings(),
                        GetAnnotationsPadding())));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(totalNutritionSum);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private string CreateCakeVectorSumScript()
        {
            var cakeNutritionVectorCombo = new Equation(
                AddComponents.Create(
                    new NumericMultiplierComponent(3,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 408, 0, 0),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Butter" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                    new NumericMultiplierComponent(2,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 455, 0, 13),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Flour" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                    new NumericMultiplierComponent(6,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 194, 50, 0),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Sugar" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                    new NumericMultiplierComponent(3,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 78, 1, 6),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Eggs" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding())))),
                new AnnotatedMatrixComponent(
                    new MatrixComponent(3, 1, 3532, 303, 44),
                    new MatrixAnnotations(
                        null,
                        false,
                        new List<string>() { "Cake Nutrition" },
                        CreateAnnotationsTextSettings(),
                        GetAnnotationsPadding())));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(cakeNutritionVectorCombo);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
        }

        private string CreateIcingVectorSumScript()
        {
            var icingNutritionVectorCombo = new Equation(
                AddComponents.Create(
                    new NumericMultiplierComponent(8,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 408, 0, 0),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Butter" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                    new NumericMultiplierComponent(0,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 455, 0, 13),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Flour" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                    new NumericMultiplierComponent(6,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 194, 50, 0),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Sugar" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding()))),
                    new NumericMultiplierComponent(6,
                        new AnnotatedMatrixComponent(new MatrixComponent(3, 1, 78, 1, 6),
                        new MatrixAnnotations(
                            null,
                            false,
                            new List<string>() { "Eggs" },
                            CreateAnnotationsTextSettings(),
                            GetAnnotationsPadding())))),
                new AnnotatedMatrixComponent(
                    new MatrixComponent(3, 1, 4896, 306, 36),
                    new MatrixAnnotations(
                        null,
                        false,
                        new List<string>() { "Icing Nutrition" },
                        CreateAnnotationsTextSettings(),
                        GetAnnotationsPadding())));

            var expressionManager = CreateExpressionManager();
            var layoutResults = expressionManager.Render(icingNutritionVectorCombo);

            var scriptCreator = new MatrixScriptCreator();
            var script = scriptCreator.CreateScript(layoutResults);

            return script;
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
            return new TextSettings("Arial", 55, TextSettingsFontSizeUnit.Pixels);
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
