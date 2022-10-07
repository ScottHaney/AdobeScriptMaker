using AdobeScriptMaker.Core;
using MatrixLayout;
using MatrixLayout.ExpressionDecorators;
using MatrixLayout.InputDescriptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Applications.Tests
{
    public class Tests
    {
        [Test]
        public void Test()
        {
            var textSettings = new TextDisplayDescription("Arial", 72);
            var matrixLayoutSettings = new MatrixLayoutDescription(
                    new MatrixBracketsDescription(2, 35),
                    new MatrixInteriorMarginsDescription(0.5f, 0.1f, 0.5f, 1),
                    12, 20, 35);

            var expressionManager = new ExpressionManager(new ExpressionDisplaySettings(
                textSettings,
                matrixLayoutSettings));

            var cakeNutrition = new MatrixComponent(new MatrixValuesDescription(3, 2, 3532, 4896, 303, 306, 44, 36));
            var cakeIcingMixture = new MultiplyComponents(
                new MatrixComponent(new MatrixValuesDescription(3, 4, 408, 455, 194, 78, 0, 0, 50, 1, 0, 13, 0, 6)),
                new MatrixComponent(new MatrixValuesDescription(4, 2, 3, 8, 2, 0, 6, 6, 3, 6)));

            var cakeNutritionVectorCombo = new Equation(
                AddComponents.Create(
                    new NumericMultiplierComponent(3, new MatrixComponent(3, 1, 408, 0, 0)),
                    new NumericMultiplierComponent(2, new MatrixComponent(3, 1, 455, 0, 13)),
                    new NumericMultiplierComponent(6, new MatrixComponent(3, 1, 194, 50, 0)),
                    new NumericMultiplierComponent(3, new MatrixComponent(3, 1, 78, 1, 6))),
                new MatrixComponent(3, 1, 3532, 303, 44));

            var icingNutritionVectorCombo = new Equation(
                AddComponents.Create(
                    new NumericMultiplierComponent(8, new MatrixComponent(3, 1, 408, 0, 0)),
                    new NumericMultiplierComponent(0, new MatrixComponent(3, 1, 455, 0, 13)),
                    new NumericMultiplierComponent(6, new MatrixComponent(3, 1, 194, 50, 0)),
                    new NumericMultiplierComponent(6, new MatrixComponent(3, 1, 78, 1, 6))),
                new MatrixComponent(3, 1, 4896, 306, 36));

            var scriptCreator = new MatrixScriptCreator();

            var layoutResults = new List<IExpressionComponent>() { cakeNutrition, cakeIcingMixture, cakeNutritionVectorCombo, icingNutritionVectorCombo }
                .Select(x => expressionManager.Render(x));

            var script = scriptCreator.CreateScript(layoutResults.ToArray());
        }
    }
}