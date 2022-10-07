using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.InputDescriptions
{
    public class MatrixLayoutDescription
    {
        public readonly MatrixBracketsDescription BracketsDescription;
        public readonly MatrixInteriorMarginsDescription InteriorMarginsDescription;
        public readonly int MultiplierSpacing;
        public readonly int MatrixMultiplicationSpacing;
        public readonly int MatrixAdditionSpacing;

        public MatrixLayoutDescription(MatrixBracketsDescription bracketsDescription,
            MatrixInteriorMarginsDescription interiorMarginsDescription,
            int multiplierSpacing = 8,
            int matrixMultiplicationSpacing = 5,
            int matrixAdditionSpacing = 15)
        {
            BracketsDescription = bracketsDescription;
            InteriorMarginsDescription = interiorMarginsDescription;
            MultiplierSpacing = multiplierSpacing;
            MatrixMultiplicationSpacing = matrixMultiplicationSpacing;
            MatrixAdditionSpacing = matrixAdditionSpacing;
        }
    }
}
