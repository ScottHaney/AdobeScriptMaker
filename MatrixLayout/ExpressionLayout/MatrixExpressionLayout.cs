using MatrixLayout.ExpressionDecorators;
using MatrixLayout.ExpressionLayout.LayoutResults;
using MatrixLayout.ExpressionLayout.Matrices;
using MatrixLayout.InputDescriptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MatrixLayout.ExpressionLayout
{
    public class MatrixExpressionLayout : IExpressionLayout
    {
        private readonly TextDisplayDescription _textSettings;
        private readonly MatrixLayoutDescription _matrixSettings;
        private readonly ITextMeasurerFactory _textMeasurerFactory;

        public MatrixExpressionLayout(TextDisplayDescription textSettings,
            MatrixLayoutDescription matrixSettings,
            ITextMeasurerFactory textMeasurerFactory)
        {
            _textSettings = textSettings;
            _matrixSettings = matrixSettings;
            _textMeasurerFactory = textMeasurerFactory;
        }

        public ILayoutResults Layout(IExpressionComponent item)
        {
            var results = LayoutComponentSwitch((dynamic)item, 0);
            CenterComponents(results);

            var items = ((LayoutResultsComposite)results).Items;
            
            return results;
        }

        private void CenterComponents(ILayoutResults layoutResults)
        {
            if (layoutResults is LayoutResultsComposite composite)
            {
                var items = composite.Items;

                var maxHeight = items.Max(x => x.BoundingBox.Height);

                foreach (var component in items)
                {
                    var diff = maxHeight - component.BoundingBox.Height;
                    if (diff > 0)
                        component.ShiftDown(diff / 2);
                }
            }            
        }

        private IEnumerable<ILayoutResults> GetComponents(ILayoutResults original)
        {
            if (original is LayoutResultsComposite composite)
            {
                if (composite.IsComponent)
                    yield return original;
                else
                {
                    foreach (var item in composite.Items)
                    {
                        foreach (var component in GetComponents(item))
                            yield return component;
                    }    
                }    
            }

            yield return original;
        }

        private ILayoutResults LayoutComponentSwitch(IExpressionComponent item, float startingLeft)
        {
            return LayoutComponent((dynamic)item, startingLeft);
        }

        private ILayoutResults LayoutComponent(Equation equation, float startingLeft)
        {
            const string EQUALS_SIGN = "=";
            var leftLayout = LayoutComponentSwitch(equation.Lhs, startingLeft);

            using (var textMeasurer = _textMeasurerFactory.Create())
            {
                var equalsSize = textMeasurer.MeasureText(EQUALS_SIGN, new Font(_textSettings.FontName, _textSettings.FontSizeInPixels, GraphicsUnit.Pixel));
                var spacing = 15;

                var equalsBox = new TextLayoutResult(new RectangleF(leftLayout.BoundingBox.Right + spacing,
                    0,
                    equalsSize.Width,
                    equalsSize.Height),
                    new TextSettings(new Font(_textSettings.FontName, _textSettings.FontSizeInPixels, GraphicsUnit.Pixel)),
                    EQUALS_SIGN);

                var rightLayout = LayoutComponentSwitch(equation.Rhs, equalsBox.Bounds.Right + spacing);

                return CombineResults(leftLayout, new LayoutResultsCollection(equalsBox), rightLayout);
            }
        }

        private ILayoutResults LayoutComponent(Expression expression, float startingLeft)
        {
            var results = new List<ILayoutResults>();
            foreach (var item in expression.Components)
            {
                results.Add(LayoutComponentSwitch(item, startingLeft));
                startingLeft = results.Last().BoundingBox.Right;
            }

            return CombineResults(results.ToArray());
        }

        private ILayoutResults LayoutComponent(AddComponents addComponents, float startingLeft)
        {
            const string PLUS_SIGN = "+";
            var leftLayout = LayoutComponentSwitch(addComponents.Lhs, startingLeft);

            using (var textMeasurer = _textMeasurerFactory.Create())
            {
                var additionSize = textMeasurer.MeasureText(PLUS_SIGN, new Font(_textSettings.FontName, _textSettings.FontSizeInPixels, GraphicsUnit.Pixel));
                var spacing = 15;

                var multiplierBox = new TextLayoutResult(new RectangleF(leftLayout.BoundingBox.Right + spacing,
                    0,
                    additionSize.Width,
                    additionSize.Height),
                    new TextSettings(new Font(_textSettings.FontName, _textSettings.FontSizeInPixels, GraphicsUnit.Pixel)),
                    PLUS_SIGN);

                var rightLayout = LayoutComponentSwitch(addComponents.Rhs, multiplierBox.Bounds.Right + spacing);

                return CombineResults(leftLayout, new LayoutResultsCollection(multiplierBox), rightLayout);
            }
        }

        private ILayoutResults LayoutComponent(MultiplyComponents multiplyComponents, float startingLeft)
        {
            var leftLayout = LayoutComponentSwitch(multiplyComponents.Lhs, startingLeft);

            var spacing = 8;
            startingLeft = leftLayout.BoundingBox.Right + spacing;

            var rightLayout = LayoutComponentSwitch(multiplyComponents.Rhs, startingLeft);

            return CombineResults(leftLayout, rightLayout);
        }

        private ILayoutResults LayoutComponent(NumericMultiplierComponent multiplierComponent, float startingLeft)
        {
            using (var textMeasurer = _textMeasurerFactory.Create())
            {
                var multiplierSize = textMeasurer.MeasureText(multiplierComponent.Mult.ToString(), new Font(_textSettings.FontName, _textSettings.FontSizeInPixels, GraphicsUnit.Pixel));
                var spacing = 5;

                var multiplierBox = new TextLayoutResult(new RectangleF(startingLeft,
                    0,
                    multiplierSize.Width,
                    multiplierSize.Height),
                    new TextSettings(new Font(_textSettings.FontName, _textSettings.FontSizeInPixels, GraphicsUnit.Pixel)),
                    multiplierComponent.Mult.ToString());

                startingLeft += multiplierSize.Width + spacing;
                var innerResult = LayoutComponentSwitch(multiplierComponent.Target, startingLeft);

                return CombineResults(new LayoutResultsCollection(multiplierBox), innerResult);
            }
        }

        private ILayoutResults LayoutComponent(MatrixComponent matrixComponent, float startingLeft)
        {
            var layout = new SizedToEntriesMatrixEntriesLayout(_matrixSettings.InteriorMarginsDescription,
                matrixComponent.Rows,
                matrixComponent.Columns);

            using (var textMeasurer = _textMeasurerFactory.Create())
            {
                return layout.GetLayoutResultWithBrackets(new SizedMatrixEntriesLayoutInputParams(textMeasurer, new Font(_textSettings.FontName, _textSettings.FontSizeInPixels, GraphicsUnit.Pixel), matrixComponent.Entries), _matrixSettings.BracketsDescription, startingLeft);
            }
        }

        private ILayoutResults CombineResults(params ILayoutResults[] results)
        {
            var expandedResults = new List<ILayoutResults>();
            foreach (var result in results)
            {
                if (result is LayoutResultsComposite composite)
                    expandedResults.AddRange(composite.Items);
                else
                    expandedResults.Add(result);
            }

            return new LayoutResultsComposite(expandedResults.ToArray());
        }
    }

    public class ExpressionLayoutResults
    {
        private readonly ILayoutResults _layoutResults;
        private readonly float _maxItemHeight;

        public ExpressionLayoutResults(ILayoutResults layoutResults,
            float maxItemHeight)
        {
            _layoutResults = layoutResults;
            _maxItemHeight = maxItemHeight;
        }
    }
}
