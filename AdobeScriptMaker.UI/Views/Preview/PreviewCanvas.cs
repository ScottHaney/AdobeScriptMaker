using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AdobeScriptMaker.UI.Views.Preview
{
    public class PreviewCanvas : UIElement
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
            if (Primitives == null)
                return;

            foreach (var primitive in Primitives.Items)
            {
                primitive.Draw(drawingContext);
            }
        }
    }

    public class PreviewCanvasPrimitives
    {
        public readonly IEnumerable<IPreviewCanvasPrimitive> Items;

        public PreviewCanvasPrimitives(IEnumerable<IPreviewCanvasPrimitive> items)
        {
            Items = items;
        }
    }

    public interface IPreviewCanvasPrimitive
    {
        void Draw(DrawingContext drawingContext);
    }

    public class PreviewCanvasLinePrimitive : IPreviewCanvasPrimitive
    {
        private readonly Point Start;
        private readonly Point End;

        public PreviewCanvasLinePrimitive(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public void Draw(DrawingContext drawingContext)
        {
            drawingContext.DrawLine(new Pen(Brushes.Black, 1), Start, End);
        }
    }
}
