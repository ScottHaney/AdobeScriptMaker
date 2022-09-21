using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionLayout.Matrices
{
    public interface IMatrixEntriesLayout
    {
        MatrixEntriesLayoutResult GetLayoutResult(IMatrixEntriesLayoutInputParams inputParams, float startingLeft = 0);
        MatrixEntriesLayoutResult GetLayoutResultWithBrackets(IMatrixEntriesLayoutInputParams inputParams, float bracketThickness, float startingLeft = 0);
    }
}
