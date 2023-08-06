using AdobeScriptMaker.UI.Views.Preview.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace AdobeScriptMaker.UI.Views.Preview
{
    public class PreviewCanvas : Control
    {
        public PreviewCanvasPrimitives Primitives
        {
            get { return (PreviewCanvasPrimitives)GetValue(PrimitivesProperty); }
            set { SetValue(PrimitivesProperty, value); }
        }

        public static readonly DependencyProperty PrimitivesProperty =
            DependencyProperty.Register("Primitives", typeof(PreviewCanvasPrimitives), typeof(PreviewCanvas),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, DesiredSize.Width, DesiredSize.Height));

            if (Primitives == null || !Primitives.Items.Any())
                return;

            foreach (var primitive in Primitives.Items)
            {
                primitive.Draw(drawingContext);
            }
        }
    }
}
