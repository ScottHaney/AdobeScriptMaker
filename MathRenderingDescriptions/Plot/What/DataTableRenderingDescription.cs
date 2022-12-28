using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;

namespace MathRenderingDescriptions.Plot.What
{
    public class DataTableRenderingDescription : IWhatToRender
    {
        public readonly DataTableData Data;

        public string[] NumericToStringFormats { get; set; }

        public TextSettings TextSettings { get; set; } = new TextSettings("Tahoma", 50, TextSettingsFontSizeUnit.Pixels);

        public PointF TopLeft { get; set; } = new PointF(0, 0);

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

        public IEnumerable<List<double>> AllDataInMatrixOrder()
        {
            foreach (var row in _data)
                yield return row;
        }
    }
}
