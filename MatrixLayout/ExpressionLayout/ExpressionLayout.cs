using MatrixLayout.ExpressionDecorators;
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

        public void Layout(IEquationComponent item)
        {
            throw new NotImplementedException();
        }

        private MatrixEntriesLayoutResult Layout(MatrixComponent matrixComponent)
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
