using AdobeScriptMaker.Core.ComponentsConverters;
using DirectRendering;
using DirectRendering.Drawing;
using DirectRendering.Plotting;
using MathDescriptions.Plot;
using MathDescriptions.Plot.Calculus;
using MathDescriptions.Plot.Functions;
using MathRenderingDescriptions.Plot;
using MathRenderingDescriptions.Plot.What;
using MathRenderingDescriptions.Plot.What.RiemannSums;
using NUnit.Framework;
using RenderingDescriptions;
using RenderingDescriptions.When;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdobeScriptMaker.Core.Tests
{
    public class ComponentsScriptCreatorTests
    {
        [Test]
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
            var drawingSequence = new DrawingSequence(axes);

            var converter = new AdobeComponentsConverter();
            var converted = converter.Convert(drawingSequence);

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }

        [Test]
        public void CreatesTriangleIntegralDisplay()
        {
            var plotLayoutDescription = new PlotLayoutDescription(
                new PlotAxesLayoutDescription(
                    new PlotAxisLayoutDescription(800, 0, 5),
                    new PlotAxisLayoutDescription(800, 0, 5)), new PointF(100, 100));

            var axes = new AxesRenderingDescription(plotLayoutDescription);

            var function = new FunctionRenderingDescription(plotLayoutDescription,
                x => x);

            var areaUnderFunction = new AreaUnderFunctionRenderingDescription(function);

            var sumsProvider = new SumsProvider(1, 2, 4, 8, 16);
            var riemannSums = new RiemannSumsRenderingDescription(function,
                new FitToDuration(sumsProvider.NumSums, 5),
                sumsProvider);

            var riemannSumsMetadata = riemannSums.GetMetadata();
            var dataTableData = new DataTableData(new List<List<double>>()
            {
                riemannSumsMetadata.SumsDetails.Select(x => (double)x.NumSums).ToList(),
                riemannSumsMetadata.SumsDetails.Select(x => x.TotalArea).ToList()
            });

            var dataTable = new DataTableRenderingDescription(dataTableData) { NumericToStringFormat = "N1" };

            var axesToRender = new RenderingDescription(axes, new AbsoluteTiming(0), null);
            var functionToRender = new RenderingDescription(function, new AbsoluteTiming(2.1), null);
            var aufToRender = new RenderingDescription(areaUnderFunction, new AbsoluteTiming(4), null);
            var rsToRender = new RenderingDescription(riemannSums, new AbsoluteTiming(5), null);
            var dtToRender = new RenderingDescription(dataTable, new AbsoluteTiming(0), null);

            var converter = new UpdatedComponentsConverter();
            var converted = converter.Convert(new List<RenderingDescription>() { axesToRender, functionToRender, aufToRender, rsToRender, dtToRender }, new AbsoluteTiming(15));

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }

        [Test]
        public void CreatesPlotOfXSquaredPlusOne()
        {
            var plotDescription = new PlotDescription(
                new AxisRangeDescription(0, 10),
                new AxisRangeDescription(0, 100),
                new FunctionDescription(x => Math.Pow(x, 2) + 1));

            var plot = new Plot(plotDescription, new Rectangle(0, 0, 500, 500));
            var drawingSequence = new DrawingSequence(plot);

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
        }
    }
}
