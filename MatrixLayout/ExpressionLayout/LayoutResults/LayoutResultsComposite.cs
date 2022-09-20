using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixLayout.ExpressionLayout.LayoutResults
{
    public class LayoutResultsComposite : ILayoutResults
    {
        private readonly ILayoutResults[] _items;

        public LayoutResultsComposite(params ILayoutResults[] items)
        {
            _items = items ?? Array.Empty<ILayoutResults>();
        }

        public IEnumerable<ILayoutResult> GetResults()
        {
            return _items.SelectMany(x => x.GetResults());
        }
    }
}
