using MatrixLayout.ExpressionDecorators;
using MatrixLayout.ExpressionLayout.LayoutResults;
using MatrixLayout.ExpressionLayout.Matrices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout.ExpressionLayout
{
    public class ExpressionLayout : IExpressionLayout
    {
        private readonly Font _font;
        private readonly float _bracketThickness;

        private readonly float _innerMatrixEntriesPadding;
        private readonly float _matrixRowGap;
        private readonly float _matrixColumnGap;

        public ExpressionLayout(Font font,
            float bracketThickness,
            float innerMatrixEntriesPadding = 0.1f,
            float matrixRowGap = 0.8f,
            float matrixColumnGap = 0.5f)
        {
            _font = font;
            _bracketThickness = bracketThickness;

            _innerMatrixEntriesPadding = innerMatrixEntriesPadding;
            _matrixRowGap = matrixRowGap;
            _matrixColumnGap = matrixColumnGap;
        }

        public ILayoutResults Layout(IExpressionComponent item)
        {
            return LayoutComponentSwitch((dynamic)item, 0);
        }

        private ILayoutResults LayoutComponentSwitch(IExpressionComponent item, float startingLeft)
        {
            return LayoutComponent((dynamic)item, startingLeft);
        }

        private ILayoutResults LayoutComponent(Equation equation, float startingLeft)
        {
            var leftLayout = LayoutComponentSwitch(equation.Lhs, startingLeft);

            using (var textMeasurer = new TextMeasurer())
            {
                var equalsSize = textMeasurer.MeasureText("=", _font);
                var spacing = 15;

                var equalsBox = new TextLayoutResult(new RectangleF(leftLayout.BoundingBox.Right + spacing,
                    0,
                    equalsSize.Width,
                    equalsSize.Height));

                var rightLayout = LayoutComponentSwitch(equation.Rhs, equalsBox.Bounds.Right + spacing);

                return new LayoutResultsComposite(leftLayout, new LayoutResultsCollection(equalsBox), rightLayout);
            }
        }

        private ILayoutResults LayoutComponent(AddComponents addComponents, float startingLeft)
        {
            var leftLayout = LayoutComponentSwitch(addComponents.Lhs, startingLeft);

            using (var textMeasurer = new TextMeasurer())
            {
                var additionSize = textMeasurer.MeasureText("+", _font);
                var spacing = 15;

                var multiplierBox = new TextLayoutResult(new RectangleF(leftLayout.BoundingBox.Right + spacing,
                    0,
                    additionSize.Width,
                    additionSize.Height));

                var rightLayout = LayoutComponentSwitch(addComponents.Rhs, multiplierBox.Bounds.Right + spacing);

                return new LayoutResultsComposite(leftLayout, new LayoutResultsCollection(multiplierBox), rightLayout);
            }
        }

        private ILayoutResults LayoutComponent(MultiplyComponents multiplyComponents, float startingLeft)
        {
            var leftLayout = LayoutComponentSwitch(multiplyComponents.Lhs, startingLeft);

            var spacing = 8;
            startingLeft = leftLayout.BoundingBox.Right + spacing;

            var rightLayout = LayoutComponentSwitch(multiplyComponents.Rhs, startingLeft);

            return new LayoutResultsComposite(leftLayout, rightLayout);
        }

        private ILayoutResults LayoutComponent(NumericMultiplierComponent multiplierComponent, float startingLeft)
        {
            using (var textMeasurer = new TextMeasurer())
            {
                var multiplierSize = textMeasurer.MeasureText(multiplierComponent.Mult.ToString(), _font);
                var spacing = 5;

                var multiplierBox = new TextLayoutResult(new RectangleF(startingLeft,
                    0,
                    multiplierSize.Width,
                    multiplierSize.Height));

                startingLeft += multiplierSize.Width + spacing;
                var innerResult = LayoutComponentSwitch(multiplierComponent.Target, startingLeft);

                return new LayoutResultsComposite(new LayoutResultsCollection(multiplierBox), innerResult);
            }
        }

        private ILayoutResults LayoutComponent(MatrixComponent matrixComponent, float startingLeft)
        {
            var layout = new SizedToEntriesMatrixEntriesLayout(_innerMatrixEntriesPadding,
                _matrixRowGap,
                _matrixColumnGap,
                matrixComponent.Rows,
                matrixComponent.Columns);

            using (var textMeasurer = new TextMeasurer())
            {
                return layout.GetLayoutResultWithBrackets(new SizedMatrixEntriesLayoutInputParams(textMeasurer, _font, matrixComponent.Entries), _bracketThickness);
            }
        }
    }
}
