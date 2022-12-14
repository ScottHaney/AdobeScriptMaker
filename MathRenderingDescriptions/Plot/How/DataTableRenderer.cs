﻿using MathRenderingDescriptions.Plot.What;
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

            var layoutResult = matrixLayout.GetLayoutResult(new SizedMatrixEntriesLayoutInputParams(
                new TextMeasurer(),
                new Font("Tahoma", 50, GraphicsUnit.Pixel),
                _description.Data.AllDataInMatrixOrder().ToArray()));

            var textSettings = new AdobeTextSettings("Tahoma", 50);

            var textControls = new List<AdobeTextComponent>();
            for (int row = 0; row < _description.Data.NumRows; row++)
            {
                for (int col = 0; col < _description.Data.NumColumns; col++)
                {
                    var entryBounds = layoutResult.GetEntryBounds(row, col);
                    textControls.Add(new AdobeTextComponent(_description.Data.GetEntry(row, col).ToString(),
                        entryBounds,
                        textSettings));
                }
            }

            return new RenderedComponents(
                textControls.Select(x => new TimedAdobeLayerComponent(x, whenToRender.Time, whenToRender.Time + 30)));
        }
    }
}