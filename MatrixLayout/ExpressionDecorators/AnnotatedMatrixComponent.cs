using MatrixLayout.ExpressionLayout.LayoutResults;
using RenderingDescriptions.What;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class AnnotatedMatrixComponent : IAddableComponent, IMultipliableComponent, INumericMultiplierCapableComponent, IExpressionComponent
    {
        public readonly MatrixComponent Matrix;
        public readonly MatrixAnnotations Annotations;

        public AnnotatedMatrixComponent(MatrixComponent matrix, MatrixAnnotations annotations)
        {
            Matrix = matrix;
            Annotations = annotations;
        }
    }

    public class MatrixAnnotations
    {
        public List<string> RowAnnotations;
        public readonly bool RowAnnotationsAreOnRight;
        public List<string> ColumnAnnotations;
        public readonly TextSettings TextSettings;
        public readonly int Padding;

        public MatrixAnnotations(List<string> rowAnnotations,
            bool rowAnnotationsAreOnRight,
            List<string> columnAnnotations,
            TextSettings textSettings,
            int padding)
        {
            RowAnnotations = rowAnnotations;
            RowAnnotationsAreOnRight = rowAnnotationsAreOnRight;
            ColumnAnnotations = columnAnnotations;
            TextSettings = textSettings;
            Padding = padding;
        }
    }
}
