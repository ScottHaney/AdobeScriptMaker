using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.InputDescriptions
{
    public class MatrixInteriorMarginsDescription
    {
        public readonly float EntriesPaddingPercentageHorizontal;
        public readonly float EntriesPaddingPercentageVertical;
        public readonly float RowGapPercentage;
        public readonly float ColumnGapPercentage;

        public MatrixInteriorMarginsDescription(float entriesPaddingPercentageHorizontal,
            float entriesPaddingPercentageVertical,
            float rowGapPercentage,
            float columnGapPercentage)
        {
            EntriesPaddingPercentageHorizontal = entriesPaddingPercentageHorizontal;
            EntriesPaddingPercentageVertical = entriesPaddingPercentageVertical;
            RowGapPercentage = rowGapPercentage;
            ColumnGapPercentage = columnGapPercentage;
        }
    }
}
