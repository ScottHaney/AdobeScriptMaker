using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MatrixLayout.InputDescriptions
{
    public class MatrixValuesDescription
    {
        public readonly int Rows;
        public readonly int Columns;
        public readonly string[] Entries;

        public MatrixValuesDescription(int rows, int columns, params double[] entries)
        {
            Rows = rows;
            Columns = columns;
            Entries = entries.Select(x => x.ToString()).ToArray();
        }

        public MatrixValuesDescription(int rows, int columns, params string[] entries)
        {
            Rows = rows;
            Columns = columns;
            Entries = entries;
        }
    }
}
