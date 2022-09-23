using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.InputDescriptions
{
    public class MatrixInteriorMarginsDescription
    {
        public readonly float EntriesPaddingPercentage;
        public readonly float RowGapPercentage;
        public readonly float ColumnGapPercentage;

        public MatrixInteriorMarginsDescription(float entriesPaddingPercentage,
            float rowGapPercentage,
            float columnGapPercentage)
        {
            EntriesPaddingPercentage = entriesPaddingPercentage;
            RowGapPercentage = rowGapPercentage;
            ColumnGapPercentage = columnGapPercentage;
        }
    }
}
