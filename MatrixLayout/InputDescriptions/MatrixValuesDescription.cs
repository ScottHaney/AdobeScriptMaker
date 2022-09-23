using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.InputDescriptions
{
    public class MatrixValuesDescription
    {
        public readonly int Rows;
        public readonly int Columns;
        public readonly double[] Entries;

        public MatrixValuesDescription(int rows, int columns, params double[] entries)
        {
            Rows = rows;
            Columns = columns;
            Entries = entries;
        }
    }
}
