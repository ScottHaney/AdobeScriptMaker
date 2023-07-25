using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace AdobeScriptMaker.UI.Views.Preview
{
    public class PreviewCanvas : FrameworkElement
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
            if (Primitives == null || !Primitives.Items.Any())
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
        Rect GetBounds();
    }

    public class PreviewCanvasLinePrimitive : IPreviewCanvasPrimitive
    {
        private readonly Point Start;
        private readonly Point End;

        public PreviewCanvasLinePrimitive(System.Drawing.Point start, System.Drawing.Point end)
        {
            Start = new Point(start.X, start.Y);
            End = new Point(end.X, end.Y);
        }

        public Rect GetBounds()
        {
            var minX = Math.Min(Start.X, End.X);
            var maxX = Math.Max(Start.X, End.X);

            var minY = Math.Min(Start.Y, End.Y);
            var maxY = Math.Max(Start.Y, End.Y);

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }

        public void Draw(DrawingContext drawingContext)
        {
            drawingContext.DrawLine(new Pen(Brushes.Black, 1), Start, End);
        }
    }
}
