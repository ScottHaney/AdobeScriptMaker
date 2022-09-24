using MatrixLayout.ExpressionLayout.LayoutResults;
using MatrixLayout.InputDescriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionLayout.Matrices
{
    public interface IMatrixEntriesLayout
    {
        MatrixEntriesLayoutResult GetLayoutResult(IMatrixEntriesLayoutInputParams inputParams, float startingLeft = 0);
        ILayoutResults GetLayoutResultWithBrackets(IMatrixEntriesLayoutInputParams inputParams, MatrixBracketsDescription bracketsSettings, float startingLeft = 0);
    }
}
