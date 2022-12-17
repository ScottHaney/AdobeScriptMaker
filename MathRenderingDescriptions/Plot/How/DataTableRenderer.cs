using MathRenderingDescriptions.Plot.What;
using MatrixLayout.ExpressionLayout.Matrices;
using RenderingDescriptions.How;
using RenderingDescriptions.When;
using System;
using System.Collections.Generic;
using System.Text;
using MatrixLayout.InputDescriptions;
using System.Linq;
using MatrixLayout;
using System.Drawing;
using AdobeComponents.Components;
using AdobeComponents.Animation;

namespace MathRenderingDescriptions.Plot.How
{
    public class DataTableRenderer : IHowToRender
    {
        private readonly DataTableRenderingDescription _description;

        public DataTableRenderer(DataTableRenderingDescription description)
        {
            _description = description;
        }

        public RenderedComponents Render(AbsoluteTiming whenToRender)
        {
            var matrixLayout = new SizedToEntriesMatrixEntriesLayout(
                new MatrixInteriorMarginsDescription(0.1f, 0.1f, 0.1f, 0.1f),
                _description.Data.NumRows,
                _description.Data.NumColumns);

            var font = _description.TextSettings.ToFont();
            var layoutResult = matrixLayout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(
                new TextMeasurer(),
                font,
                _description.Data.AllDataInMatrixOrder().Select(x => FormatNumber(x)).ToArray()));

            var textSettings = new AdobeTextSettings(font.Name, font.SizeInPoints);

            var textControls = new List<AdobeTextComponent>();
            for (int row = 0; row < _description.Data.NumRows; row++)
            {
                for (int col = 0; col < _description.Data.NumColumns; col++)
                {
                    var entryBounds = layoutResult.GetEntryBounds(row, col);
                    entryBounds = new RectangleF(entryBounds.X + _description.TopLeft.X,
                        entryBounds.Y + _description.TopLeft.Y,
                        entryBounds.Width,
                        entryBounds.Height);

                    textControls.Add(new AdobeTextComponent(FormatNumber(_description.Data.GetEntry(row, col)),
                        entryBounds,
                        textSettings));
                }
            }

            return new RenderedComponents(
                textControls.Select(x => new TimedAdobeLayerComponent(x, whenToRender.Time, whenToRender.Time + 30)));
        }

        private string FormatNumber(double number)
        {
            if (_description.NumericToStringFormat == null)
                return number.ToString();
            else
                return number.ToString(_description.NumericToStringFormat);
        }
    }
}
