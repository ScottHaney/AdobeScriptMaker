using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.InputDescriptions
{
    public class MatrixDescription
    {
        public readonly MatrixValuesDescription ValuesInfo;
        public readonly MatrixBracketsDescription BracketsInfo;
        public readonly TextDisplayDescription TextDisplayInfo;

        public MatrixDescription(MatrixValuesDescription valuesInfo,
            MatrixBracketsDescription bracketsInfo,
            TextDisplayDescription textDisplayInfo)
        {
            ValuesInfo = valuesInfo;
            BracketsInfo = bracketsInfo;
            TextDisplayInfo = textDisplayInfo;
        }
    }

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
