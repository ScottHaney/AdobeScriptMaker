using AdobeScriptMaker.Core.ComponentsConverters;
using DirectRendering;
using DirectRendering.Drawing;
using DirectRendering.Plotting;
using MathDescriptions.Plot;
using MathDescriptions.Plot.Calculus;
using MathDescriptions.Plot.Functions;
using MathRenderingDescriptions.Plot;
using MathRenderingDescriptions.Plot.How;
using MathRenderingDescriptions.Plot.How.RiemannSums;
using MathRenderingDescriptions.Plot.What;
using MathRenderingDescriptions.Plot.What.ArcLength;
using MathRenderingDescriptions.Plot.What.Helpers;
using MathRenderingDescriptions.Plot.What.RiemannSums;
using MathRenderingDescriptions.Plot.When;
using MatrixLayout.ExpressionLayout.LayoutResults;
using NUnit.Framework;
using RenderingDescriptions;
using RenderingDescriptions.Timing;
using RenderingDescriptions.What;
using RenderingDescriptions.When;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdobeScriptMaker.Core.Tests
{
    public class ComponentsScriptCreatorTests
    {
        /*[Test]
        public void CreatesAStraightLine()
        {
            var line = new PathDrawing(new Point(1, 2), new Point(100, 200));
            var drawingSequence = new DrawingSequence(line);

            var converter = new AdobeComponentsConverter();
            var converted = converter.Convert(drawingSequence);

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }

        [Test]
        public void CreatesAxes()
        {
            var axes = new PlotAxes(new Rectangle(0, 0, 100, 100));
            var drawingSequence = new DrawingSequence(axes.GetDrawings().ToArray());

            var converter = new AdobeComponentsConverter();
            var converted = converter.Convert(drawingSequence);

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }*/

        [Test]
        public void CreatesTriangleIntegralDisplay()
        {
            var plotLayoutDescription = new PlotLayoutDescription(
                new PlotAxesLayoutDescription(
                    new PlotAxisLayoutDescription(690, 0, 5),
                    new PlotAxisLayoutDescription(690, 0, 5)), new PointF(100, 300));

            var axes = new AxesRenderingDescription("Axes",
                plotLayoutDescription);

            var function = new FunctionRenderingDescription("Function",
                plotLayoutDescription,
                x => x);

            var areaUnderFunction = new AreaUnderFunctionRenderingDescription("AUC",
                function);

            var whenToRenderRiemannSums = new TimingForRender(new AbsoluteTiming(6), new AbsoluteTiming(4));

            var sumsProvider = new IntervalSegmentation(1, 2, 4, 8, 16);
            var riemannSums = new RiemannSumsRenderingDescription("RiemannSums",
                function,
                new FitToDuration(sumsProvider),
                sumsProvider);

            var riemannSumsMetadata = riemannSums.GetMetadata();
            var riemannSumsTimes = riemannSums.TimingDescription.GetTimings(whenToRenderRiemannSums.WhenToStart.Time, whenToRenderRiemannSums.RenderDuration.Time);

            var dataTableData = new DataTableData(new List<List<double>>()
            {
                riemannSumsMetadata.SumsDetails.Select(x => (double)x.NumSums).ToList(),
                riemannSumsMetadata.SumsDetails.Select(x => x.TotalArea).ToList()
            });

            var plotBounds = plotLayoutDescription.GetBounds();
            var dataTable = new DataTableRenderingDescription("DataTable",
                dataTableData)
            {
                NumericToStringFormats = new[] { "N0", "N1" },
                EntryTextSettings = new TextSettings("Graphie Light", 50, TextSettingsFontSizeUnit.Pixels),
                RowHeaderTextSettings = new TextSettings("Graphie Light", 50, TextSettingsFontSizeUnit.Pixels),
                RowHeaderValues = new List<string>() { "Rectangles", "Area" },
                TopLeft = new PointF(plotBounds.X, plotBounds.Y + plotBounds.Height + 200)
            };

            var compositionDuration = new AbsoluteTiming(30);

            var dtTimingForRender = new DataTableTimingForRender(whenToRenderRiemannSums.WhenToStart,
                new AbsoluteTiming(15),
                riemannSumsTimes
                    .Select(x => new AbsoluteTiming(x.SumIsInPlaceTime))
                    .ToArray());

            var sharedControlValues = new SharedControlValue[]
            {
                new SharedControlValue(riemannSums.GetScribbleColorControlName(), "[0, 0, 0]"),
                new SharedControlValue(riemannSums.GetLinesColorControlName(), "[0, 0, 0]"),
                new SharedControlValue(dataTable.GetFontColorControlName(), "[0, 0, 0]")
            };

            var axesToRender = new RenderingDescription(axes, new TimingForRender(new AbsoluteTiming(0), compositionDuration) { EntranceAnimationDuration = new AbsoluteTiming(0.5) }, null);
            var functionToRender = new RenderingDescription(function, new TimingForRender(new AbsoluteTiming(2.1), compositionDuration) { EntranceAnimationDuration = new AbsoluteTiming(0.5) }, null);
            var aufToRender = new RenderingDescription(areaUnderFunction, new TimingForRender(new AbsoluteTiming(4), new AbsoluteTiming(2)) { EntranceAnimationDuration = new AbsoluteTiming(0.5), ExitAnimationDuration = new AbsoluteTiming(0.5) }, null);
            var rsToRender = new RenderingDescription(riemannSums, whenToRenderRiemannSums, null);
            var dtToRender = new RenderingDescription(dataTable, dtTimingForRender, null);

            var converter = new UpdatedComponentsConverter();
            var converted = converter.Convert(new List<RenderingDescription>() { axesToRender, functionToRender, aufToRender, rsToRender, dtToRender });

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted, sharedControlValues);
        }

        [Test]
        public void CreatesShapeRiemannSumApproximation()
        {
            var plotLayoutDescription = new PlotLayoutDescription(
                new PlotAxesLayoutDescription(
                    new PlotAxisLayoutDescription(850, 0, 5),
                    new PlotAxisLayoutDescription(850, 0, 5)), new PointF(50, 300));

            var axes = new AxesRenderingDescription("Axes",
                plotLayoutDescription);

            var function = new FunctionRenderingDescription("Function",
                plotLayoutDescription,
                x => Math.Sin(x) + 3);

            var areaUnderFunctionShape = new AreaUnderFunctionShapeRenderingDescription("AUC",
                function);

            var whenToRenderRiemannSums = new TimingForRender(new AbsoluteTiming(5), new AbsoluteTiming(5));

            var sumsProvider = new IntervalSegmentation(1, 2, 4, 8, 16);
            var riemannSums = new RiemannSumsRenderingDescription("RiemannSums",
                function,
                new FitToDuration(sumsProvider),
                sumsProvider);

            var riemannSumsMetadata = riemannSums.GetMetadata();
            var riemannSumsTimes = riemannSums.TimingDescription.GetTimings(whenToRenderRiemannSums.WhenToStart.Time, whenToRenderRiemannSums.RenderDuration.Time);

            var dataTableData = new DataTableData(new List<List<double>>()
            {
                riemannSumsMetadata.SumsDetails.Select(x => (double)x.NumSums).ToList(),
                riemannSumsMetadata.SumsDetails.Select(x => x.TotalArea).ToList()
            });

            var plotBounds = plotLayoutDescription.GetBounds();
            var dataTable = new DataTableRenderingDescription("DataTable",
                dataTableData)
            {
                NumericToStringFormats = new[] { "N0", "N1" },
                EntryTextSettings = new TextSettings("Graphie Light", 50, TextSettingsFontSizeUnit.Pixels),
                RowHeaderTextSettings = new TextSettings("Graphie Light", 50, TextSettingsFontSizeUnit.Pixels),
                RowHeaderValues = new List<string>() { "Rectangles", "Area" },
                TopLeft = new PointF(plotBounds.X, plotBounds.Y + plotBounds.Height + 200)
            };

            var compositionDuration = new AbsoluteTiming(30);

            var dtTimingForRender = new DataTableTimingForRender(whenToRenderRiemannSums.WhenToStart,
                new AbsoluteTiming(15),
                riemannSumsTimes
                    .Select(x => new AbsoluteTiming(x.SumIsInPlaceTime))
                    .ToArray());

            var sharedControlValues = new SharedControlValue[]
            {
                new SharedControlValue(riemannSums.GetScribbleColorControlName(), "[0, 0, 0]"),
                new SharedControlValue(riemannSums.GetLinesColorControlName(), "[0, 0, 0]"),
                new SharedControlValue(dataTable.GetFontColorControlName(), "[0, 0, 0]")
            };

            var aufToRender = new RenderingDescription(areaUnderFunctionShape, new TimingForRender(new AbsoluteTiming(3.2), compositionDuration) { EntranceAnimationDuration = new AbsoluteTiming(0.5), ExitAnimationDuration = new AbsoluteTiming(0.5) }, null);
            var rsToRender = new RenderingDescription(riemannSums, whenToRenderRiemannSums, null);
            var dtToRender = new RenderingDescription(dataTable, dtTimingForRender, null);

            var converter = new UpdatedComponentsConverter();
            var converted = converter.Convert(new List<RenderingDescription>() { aufToRender, rsToRender, dtToRender });

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted, sharedControlValues);
        }

        [Test]
        public void CreatesArcLengthAnimation()
        {
            var plotLayoutDescription = new PlotLayoutDescription(
                new PlotAxesLayoutDescription(
                    new PlotAxisLayoutDescription(600, 0, 5),
                    new PlotAxisLayoutDescription(600, 0, 5)), new PointF(50, 100));

            var axes = new AxesRenderingDescription("Axes",
                plotLayoutDescription);

            var function = new FunctionRenderingDescription("Function",
                plotLayoutDescription,
                x => Math.Sin(2 * x));

            var derivative = new FunctionRenderingDescription("Derivative",
                plotLayoutDescription,
                x => 2 * Math.Cos(2 * x));

            var whenToRenderRiemannSums = new TimingForRender(new AbsoluteTiming(5), new AbsoluteTiming(5));

            var sumsProvider = new IntervalSegmentation(1, 2, 4, 8, 16, 32, 64);
            var riemannSums = new ArcLengthRenderingDescription("ArcLength",
                function,
                derivative,
                new FitToDuration(sumsProvider),
                sumsProvider);

            var compositionDuration = new AbsoluteTiming(30);

            var sharedControlValues = new SharedControlValue[]
            {
                new SharedControlValue(riemannSums.GetLinesColorControlName(), "[0, 0, 0]")
            };

            var arcLengthToRender = new RenderingDescription(riemannSums, whenToRenderRiemannSums, null);

            var converter = new UpdatedComponentsConverter();
            var converted = converter.Convert(new List<RenderingDescription>() { arcLengthToRender });

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted, sharedControlValues);
        }

        [Test]
        public void CreatesPolarFunction()
        {
            var plotLayoutDescription = new PlotLayoutDescription(
                new PlotAxesLayoutDescription(
                    new PlotAxisLayoutDescription(850, 0, 5),
                    new PlotAxisLayoutDescription(850, 0, 5)), new PointF(50, 300));

            var polarInfinitySign = new Func<double, double>(x => 2 + 2 * Math.Cos(2 * x));
            var function = new PolarFunctionRenderingDescription("Function",
                plotLayoutDescription,
                polarInfinitySign,
                0,
                4 * Math.PI);

            var compositionDuration = new AbsoluteTiming(30);

            var functionToRender = new RenderingDescription(function, new TimingForRender(new AbsoluteTiming(3.2), compositionDuration) { EntranceAnimationDuration = new AbsoluteTiming(0.5), ExitAnimationDuration = new AbsoluteTiming(0.5) }, null);

            var converter = new UpdatedComponentsConverter();
            var converted = converter.Convert(new List<RenderingDescription>() { functionToRender });

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }

        /*[Test]
        public void CreatesPlotOfXSquaredPlusOne()
        {
            var plotDescription = new PlotDescription(
                new AxisRangeDescription(0, 10),
                new AxisRangeDescription(0, 100),
                new FunctionDescription(x => Math.Pow(x, 2) + 1));

            var plot = new Plot(plotDescription, new Rectangle(0, 0, 500, 500));
            var drawingSequence = new DrawingSequence(plot.GetDrawings().ToArray());

            var converter = new AdobeComponentsConverter();
            var converted = converter.Convert(drawingSequence);

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }

        [Test]
        public void CreatesAreaUnderPlotOfXSquaredPlusOne()
        {
            var functionToPlot = new FunctionDescription(x => Math.Pow(x, 2) + 1);
            var plotDescription = new PlotDescription(
                new AxisRangeDescription(0, 10),
                new AxisRangeDescription(0, 100),
                functionToPlot);

            plotDescription.Decorations.Add(new AreaUnderFunctionDescription(functionToPlot, 0, 8));

            var plot = new Plot(plotDescription, new Rectangle(0, 0, 500, 500));
            var drawingSequence = new DrawingSequence(plot.GetDrawings().ToArray());

            var converter = new AdobeComponentsConverter();
            var converted = converter.Convert(drawingSequence);

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }

        [Test]
        public void CreatesRiemannSum()
        {
            var functionToPlot = new FunctionDescription(x => Math.Pow(x, 2) + 1);
            var plotDescription = new PlotDescription(
                new AxisRangeDescription(0, 10),
                new AxisRangeDescription(0, 100),
                functionToPlot);

            plotDescription.Decorations.Add(new RiemannSumDescription(functionToPlot, 4, 0, 8,
                new RiemannSumAnimationInfo(1, 3)));

            var plot = new Plot(plotDescription, new Rectangle(0, 0, 500, 500));
            var drawingSequence = new DrawingSequence(plot.GetDrawings().ToArray());

            var converter = new AdobeComponentsConverter();
            var converted = converter.Convert(drawingSequence);

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }

        [Test]
        public void CreatesRiemannSums()
        {
            var functionToPlot = new FunctionDescription(x => x);
            var plotDescription = new PlotDescription(
                new AxisRangeDescription(0, 6),
                new AxisRangeDescription(0, 6),
                functionToPlot);

            var plotStartX = 0;
            var plotEndX = 5;

            plotDescription.Decorations.Add(new AreaUnderFunctionDescription(functionToPlot, plotStartX, plotEndX));

            plotDescription.Decorations.Add(new RiemannSumsDescription(
                new RiemannSumDescription(functionToPlot, 1, plotStartX, plotEndX, new RiemannSumAnimationInfo(1, 3)),
                5));

            var plot = new Plot(plotDescription, new Rectangle(0, 0, 800, 800));
            var drawingSequence = new DrawingSequence(plot.GetDrawings().Where(x => !(x is TimingContext)).ToArray(),
                plot.GetDrawings().OfType<TimingContext>().ToArray());

            var converter = new AdobeComponentsConverter();
            var converted = converter.Convert(drawingSequence);

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }*/
    }
}
