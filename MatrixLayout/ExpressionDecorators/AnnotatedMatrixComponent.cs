using MatrixLayout.ExpressionLayout.LayoutResults;
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
        public readonly TextSettings TextSettings;
        public readonly float Padding;

        public MatrixAnnotations(List<string> rowAnnotations,
            TextSettings textSettings,
            float padding)
        {
            RowAnnotations = rowAnnotations;
            TextSettings = textSettings;
            Padding = padding;
        }
    }
}
