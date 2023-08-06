using AdobeScriptMaker.UI.Core.ScriptBuilder.Parameters;
using MathRenderingDescriptions.Plot;
using MathRenderingDescriptions.Plot.What;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AdobeScriptMaker.UI.Core.DataModels
{
    public class AxesDataModel : IScriptComponentDataModel
    {
        public IEnumerable<IScriptBuilderParameter> Parameters { get; }

        public AxesDataModel()
        {
            Parameters = new List<IScriptBuilderParameter>()
            {
                CreateParam("X Length", 100),
                CreateParam("X Min", 0),
                CreateParam("X Max", 100),
                CreateParam("Y Length", 100),
                CreateParam("Y Min", 0),
                CreateParam("Y Max", 100),
                CreateParam("X Top", 0),
                CreateParam("Y Top", 0)
            };
        }

        public IWhatToRender ToRenderingData()
        {
            var xAxisLayout = new PlotAxisLayoutDescription(GetNumericValue("X Length"),
                GetNumericValue("X Min"),
                GetNumericValue("X Max"));

            var yAxisLayout = new PlotAxisLayoutDescription(GetNumericValue("Y Length"),
                GetNumericValue("Y Min"),
                GetNumericValue("Y Max"));

            var topLeft = new PointF((float)GetNumericValue("X Top"),
                (float)GetNumericValue("Y Top"));

            var layout = new PlotLayoutDescription(
                new PlotAxesLayoutDescription(
                    xAxisLayout,
                    yAxisLayout),
                topLeft);

            return new AxesRenderingDescription("axes",
                layout);
        }

        private ScriptBuilderNumericParameter CreateParam(string name, double defaultValue)
        {
            return new ScriptBuilderNumericParameter()
            {
                Name = name,
                MinValue = double.MinValue,
                MaxValue = double.MaxValue,
                Value = defaultValue
            };
        }

        private double GetNumericValue(string name)
        {
            var param = (ScriptBuilderNumericParameter)Parameters.First(x => x.Name == name);
            return param.Value;
        }
    }
}
