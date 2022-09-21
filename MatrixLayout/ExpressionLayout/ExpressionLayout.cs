﻿using MatrixLayout.ExpressionDecorators;
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

        private ILayoutResults LayoutComponent(MultiplyComponents multiplyComponents, float startingLeft)
        {
            var leftLayout = (ILayoutResults)LayoutComponent((dynamic)multiplyComponents.Lhs, startingLeft);

            var spacing = 8;
            startingLeft = leftLayout.BoundingBox.Right + spacing;

            var rightLayout = (ILayoutResults)LayoutComponent((dynamic)multiplyComponents.Rhs, startingLeft);

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
