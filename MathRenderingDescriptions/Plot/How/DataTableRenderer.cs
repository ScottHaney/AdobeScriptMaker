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
using MatrixLayout.ExpressionDecorators;
using MatrixLayout.ExpressionLayout;
using MatrixLayout.ExpressionLayout.LayoutResults;

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

            var entryValues = _description.Data.AllDataInMatrixOrder().SelectMany((x, rowIndex) => x.Select(y => FormatNumber(y, rowIndex))).ToArray();

            var annotatedMatrix = new AnnotatedMatrixComponent(
                new MatrixComponent(_description.Data.NumRows, _description.Data.NumColumns, entryValues),
                new MatrixAnnotations(new List<string>() { "Rectangles", "Area" }, false, new List<string>(), _description.RowHeaderTextSettings, 0));

            var matrixTextSettings = new TextDisplayDescription(_description.EntryTextSettings.FontName, (int)_description.EntryTextSettings.FontSize);
            var matrixLayoutSettings = new MatrixLayoutDescription(
                    new MatrixBracketsDescription(0, 0),
                    new MatrixInteriorMarginsDescription(0, 0, 0, 0),
                    0, 0, 0);

            var layout = new MatrixExpressionLayout(matrixTextSettings, matrixLayoutSettings, new TextMeasurerFactory());
            var layoutResults = layout.Layout(annotatedMatrix);

            var layoutResultsComponents = layoutResults.GetResults();

            //Annotations can lay out with a negative x value so add an offset to make sure the table gets positioned where the description wants it to go
            var leftOffset = Math.Abs(layoutResultsComponents.Min(x => x.Bounds.Left));

            var entryFont = _description.EntryTextSettings.ToFont();
            var entryTextSettings = new AdobeTextSettings(entryFont.Name, entryFont.SizeInPoints);

            var components = new List<TimedAdobeLayerComponent>();
            foreach (var layoutResultComponent in layoutResultsComponents.OfType<MatrixEntryLayoutResult>())
            {
                var entryBounds = layoutResultComponent.Bounds;
                entryBounds = new RectangleF(entryBounds.X + leftOffset + _description.TopLeft.X,
                    entryBounds.Y + _description.TopLeft.Y,
                    entryBounds.Width,
                    entryBounds.Height);

                var row = layoutResultComponent.Row;
                var col = layoutResultComponent.Column;

                var textControl = new AdobeTextComponent(FormatNumber(_description.Data.GetEntry(row, col), row),
                    entryBounds.Size,
                    new AdobeSliderControlRef(entryBounds.Left, "thisComp", "Shared Controls Layer", _description.GetColumnSpacingControlName()) { SliderMult = col },
                    new AdobeSliderControlRef(entryBounds.Top, "thisComp", "Shared Controls Layer", _description.GetRowSpacingControlName()) { SliderMult = row },
                    entryTextSettings)
                {
                    FontColor = new AdobeColorControlRef("thisComp", "Shared Controls Layer", _description.GetFontColorControlName())
                };

                components.Add(new TimedAdobeLayerComponent(textControl, dataTableTiming.ColumnTimings[col].Time, timing.WhenToStart.Time + timing.RenderDuration.Time));
            }

            var rowHeadersFont = _description.RowHeaderTextSettings.ToFont();
            var rowHeadersTextSettings = new AdobeTextSettings(rowHeadersFont.Name, rowHeadersFont.SizeInPoints);

            foreach (var textResult in layoutResultsComponents.OfType<TextLayoutResult>())
            {
                var entryBounds = textResult.Bounds;
                entryBounds = new RectangleF(entryBounds.X + leftOffset + _description.TopLeft.X,
                    entryBounds.Y + _description.TopLeft.Y,
                    entryBounds.Width,
                    entryBounds.Height);

                var textControl = new AdobeTextComponent(textResult.Text,
                    entryBounds.Size,
                    new AdobeSliderControlRef(entryBounds.Left, "thisComp", "Shared Controls Layer", _description.GetRowHeaderSpacingControlName()),
                    new AdobeSliderControlRef(entryBounds.Top, "thisComp", "Shared Controls Layer", _description.GetRowSpacingControlName()) { SliderMult = ((RowAnnotationMetadata)textResult.Metadata).Row },
                    rowHeadersTextSettings)
                {
                    FontColor = new AdobeColorControlRef("thisComp", "Shared Controls Layer", _description.GetFontColorControlName())
                };

                components.Add(new TimedAdobeLayerComponent(textControl, timing.WhenToStart.Time, timing.WhenToStart.Time + timing.RenderDuration.Time));
            }

            /*for (int col = 0; col < _description.Data.NumColumns; col++)
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
                        new AdobeSliderControlRef(entryBounds.Left, "thisComp", "Shared Controls Layer", _description.GetColumnSpacingControlName()) { SliderMult = col },
                        new AdobeSliderControlRef(entryBounds.Top, "thisComp", "Shared Controls Layer", _description.GetRowSpacingControlName()) { SliderMult = row },
                        textSettings)
                    {
                        FontColor = new AdobeColorControlRef("thisComp", "Shared Controls Layer", _description.GetFontColorControlName())
                    };

                    components.Add(new TimedAdobeLayerComponent(textControl, dataTableTiming.ColumnTimings[col].Time, timing.WhenToStart.Time + timing.RenderDuration.Time));
                }
            }*/

            var fontColorControl = new AdobeSharedColorControl(_description.GetFontColorControlName());
            components.Add(new TimedAdobeLayerComponent(fontColorControl, timing.WhenToStart.Time, timing.WhenToStart.Time + timing.RenderDuration.Time));

            var columnSpacingSlider = new AdobeSliderControl() { Name = _description.GetColumnSpacingControlName() };
            components.Add(new TimedAdobeLayerComponent(columnSpacingSlider, timing.WhenToStart.Time, timing.WhenToStart.Time + timing.RenderDuration.Time));

            var rowSpacingSlider = new AdobeSliderControl() { Name = _description.GetRowSpacingControlName() };
            components.Add(new TimedAdobeLayerComponent(rowSpacingSlider, timing.WhenToStart.Time, timing.WhenToStart.Time + timing.RenderDuration.Time));

            var rowHeaderSpacingSlider = new AdobeSliderControl() { Name = _description.GetRowHeaderSpacingControlName() };
            components.Add(new TimedAdobeLayerComponent(rowHeaderSpacingSlider, timing.WhenToStart.Time, timing.WhenToStart.Time + timing.RenderDuration.Time));

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
