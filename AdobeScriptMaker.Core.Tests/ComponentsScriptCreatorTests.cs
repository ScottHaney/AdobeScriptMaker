using DirectRendering.Drawing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DirectRendering;
using AdobeScriptMaker.Core.ComponentsConverters;
using DirectRendering.Plotting;
using MathDescriptions.Plot;
using MathDescriptions.Plot.Functions;
using System.Linq;
using MathDescriptions.Plot.Calculus;

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
            var functionToPlot = new FunctionDescription(x => Math.Pow(x, 2) + 1);
            var plotDescription = new PlotDescription(
                new AxisRangeDescription(0, 10),
                new AxisRangeDescription(0, 100),
                functionToPlot);

            plotDescription.Decorations.Add(new RiemannSumsDescription(
                new RiemannSumDescription(functionToPlot, 1, 0, 8, new RiemannSumAnimationInfo(1, 3)),
                7));

            var plot = new Plot(plotDescription, new Rectangle(0, 0, 500, 500));
            var drawingSequence = new DrawingSequence(plot.GetDrawings().Where(x => !(x is TimingContext)).ToArray(),
                plot.GetDrawings().OfType<TimingContext>().ToArray());

            var converter = new AdobeComponentsConverter();
            var converted = converter.Convert(drawingSequence);

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }
    }
}
