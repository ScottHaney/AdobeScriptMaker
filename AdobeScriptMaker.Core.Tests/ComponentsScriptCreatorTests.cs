using DirectRendering.Drawing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DirectRendering;
using AdobeScriptMaker.Core.ComponentsConverters;
using DirectRendering.Plotting;

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
    }
}
