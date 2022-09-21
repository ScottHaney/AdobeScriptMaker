using MatrixLayout.ExpressionDecorators;
using MatrixLayout.ExpressionLayout.LayoutResults;
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
            return LayoutComponent((dynamic)item);
        }

        private ILayoutResults LayoutComponent(MultiplyComponents multiplyComponents)
        {
            var leftLayout = LayoutComponent((dynamic)multiplyComponents.Lhs);
            var rightLayout = LayoutComponent((dynamic)multiplyComponents.Rhs);

            throw new NotImplementedException();
        }

        private ILayoutResults LayoutComponent(NumericMultiplierComponent multiplierComponent)
        {
            var innerResult = Layout(multiplierComponent.Target);

            using (var textMeasurer = new TextMeasurer())
            {
                var multiplierSize = textMeasurer.MeasureText(multiplierComponent.Mult.ToString(), _font);

                var spacing = 5;

                var entryBoundary = innerResult.BoundingBox;
                var multiplierBox = new TextLayoutResult(new RectangleF(entryBoundary.Left - multiplierSize.Width - spacing,
                    0,
                    multiplierSize.Width,
                    multiplierSize.Height));

                return new LayoutResultsComposite(new LayoutResultsCollection(multiplierBox), innerResult);
            }
        }

        private MatrixEntriesLayoutResult LayoutComponent(MatrixComponent matrixComponent)
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
