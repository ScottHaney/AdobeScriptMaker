﻿using DirectRendering.Drawing;
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
                new PolynomialDescription(new PolynomialTermDescription(1, 2), new PolynomialTermDescription(1, 0)));

            var plot = new Plot(plotDescription, new Rectangle(0, 0, 500, 500));
            var drawingSequence = new DrawingSequence(plot);

            var converter = new AdobeComponentsConverter();
            var converted = converter.Convert(drawingSequence);

            var scriptCreator = new ComponentsScriptCreator();
            var script = scriptCreator.Visit(converted);
        }
    }
}
