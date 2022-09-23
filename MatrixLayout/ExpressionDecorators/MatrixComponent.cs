using MatrixLayout.InputDescriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class MatrixComponent : IAddableComponent, IMultipliableComponent, INumericMultiplierCapableComponent, IExpressionComponent
    {
        public readonly int Rows;
        public readonly int Columns;
        public readonly double[] Entries;

        public MatrixComponent(int rows, int columns, params double[] entries)
        {
            Rows = rows;
            Columns = columns;
            Entries = entries;
        }

        public MatrixComponent(MatrixValuesDescription valuesDescription)
        {
            Rows = valuesDescription.Rows;
            Columns = valuesDescription.Columns;
            Entries = valuesDescription.Entries;
        }
    }
}
