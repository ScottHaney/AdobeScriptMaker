using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MathRenderingDescriptions.Plot.What
{
    public class DataTableRenderingDescription : IWhatToRender
    {
        public readonly DataTableData Data;

        public DataTableRenderingDescription(DataTableData data)
        {
            Data = data;
        }
    }

    public class DataTableData
    {
        public readonly int NumRows;
        public readonly int NumColumns;

        private readonly List<List<double>> _data;

        public DataTableData(List<List<double>> data)
        {
            _data = data;
            NumRows = data.Count;
            NumColumns = data.First().Count;
        }

        public double GetEntry(int row, int column)
        {
            return _data[row][column];
        }

        public IEnumerable<double> AllDataInMatrixOrder()
        {
            return _data.SelectMany(x => x);
        }
    }
}
