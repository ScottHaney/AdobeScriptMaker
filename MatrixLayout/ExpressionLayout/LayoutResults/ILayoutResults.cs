using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public interface ILayoutResults
    {
        IEnumerable<ILayoutResult> GetResults();
    }
}
