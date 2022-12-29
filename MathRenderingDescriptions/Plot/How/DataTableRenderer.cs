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
using RenderingDescriptions.Timing;
using AdobeComponents.CommonValues;

namespace MathRenderingDescriptions.Plot.How
{
    public class DataTableRenderer : IHowToRender
    {
        private readonly DataTableRenderingDescription _description;

        public DataTableRenderer(DataTableRenderingDescription description)
        {
            _description = description;
        }

        public RenderedComponents Render(ITimingForRender timing)
        {
            var dataTableTiming = (DataTableTimingForRender)timing;

            var matrixLayout = new SizedToEntriesMatrixEntriesLayout(
                new MatrixInteriorMarginsDescription(0.1f, 0.1f, 0.1f, 0.1f),
                _description.Data.NumRows,
                _description.Data.NumColumns);

            var font = _description.TextSettings.ToFont();
            var layoutResult = matrixLayout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(
                new TextMeasurer(),
                font,
                _description.Data.AllDataInMatrixOrder().SelectMany((x, rowIndex) => x.Select(y => FormatNumber(y, rowIndex))).ToArray()));

            var textSettings = new AdobeTextSettings(font.Name, font.SizeInPoints);

            var components = new List<TimedAdobeLayerComponent>();
            for (int col = 0; col < _description.Data.NumColumns; col++)
            {
                for (int row = 0; row < _description.Data.NumRows; row++)
                {
                    var entryBounds = layoutResult.GetEntryBounds(row, col);
                    entryBounds = new RectangleF(entryBounds.X + _description.TopLeft.X,
                        entryBounds.Y + _description.TopLeft.Y,
                        entryBounds.Width,
                        entryBounds.Height);

                    var textControl = new AdobeTextComponent(FormatNumber(_description.Data.GetEntry(row, col), row),
                        entryBounds.Size,
                        new AdobeSliderControlRef(entryBounds.Left, "thisComp", "Shared Controls Layer", _description.GetColumnSpacingControlName()),
                        new AdobeSliderValue(entryBounds.Top),
                        textSettings)
                    {
                        FontColor = new AdobeColorControlRef("thisComp", "Shared Controls Layer", _description.GetFontColorControlName())
                    };

                    components.Add(new TimedAdobeLayerComponent(textControl, dataTableTiming.ColumnTimings[col].Time, timing.WhenToStart.Time + timing.RenderDuration.Time));
                }
            }

            var fontColorControl = new AdobeSharedColorControl(_description.GetFontColorControlName());
            components.Add(new TimedAdobeLayerComponent(fontColorControl, timing.WhenToStart.Time, timing.WhenToStart.Time + timing.RenderDuration.Time));

            var columnSpacingSlider = new AdobeSliderControl() { Name = _description.GetColumnSpacingControlName() };
            components.Add(new TimedAdobeLayerComponent(columnSpacingSlider, timing.WhenToStart.Time, timing.WhenToStart.Time + timing.RenderDuration.Time));

            return new RenderedComponents(components);
        }

        private string FormatNumber(double number, int rowIndex)
        {
            if (_description.NumericToStringFormats[rowIndex] == null)
                return number.ToString();
            else
                return number.ToString(_description.NumericToStringFormats[rowIndex]);
        }
    }

    public class DataTableTimingForRender : ITimingForRender
    {
        public AbsoluteTiming WhenToStart { get; }

        public AbsoluteTiming RenderDuration { get; }

        public AbsoluteTiming EntranceAnimationDuration { get; set; }

        public AbsoluteTiming ExitAnimationDuration { get; set; }

        public AbsoluteTiming[] ColumnTimings { get; set; }

        public DataTableTimingForRender(AbsoluteTiming whenToStart,
            AbsoluteTiming renderDuration,
            AbsoluteTiming[] columnTimings)
        {
            WhenToStart = whenToStart;
            RenderDuration = renderDuration;
            ColumnTimings = columnTimings;
        }
    }
}
