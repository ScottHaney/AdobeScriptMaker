using MatrixLayout.ExpressionLayout.LayoutResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout
{
    public interface IMatrixEntriesLayout
    {
        MatrixEntriesLayoutResult GetLayoutResult(IMatrixEntriesLayoutInputParams inputParams);
        MatrixEntriesLayoutResult GetLayoutResultWithBrackets(IMatrixEntriesLayoutInputParams inputParams, float bracketThickness);
    }
}
