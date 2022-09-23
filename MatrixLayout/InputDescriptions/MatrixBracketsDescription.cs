using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.InputDescriptions
{
    public class MatrixBracketsDescription
    {
        public readonly float Thickness;
        public readonly float PincerWidthPercentage;

        public MatrixBracketsDescription(float thickness, float pincerWidthPercentage)
        {
            Thickness = thickness;
            PincerWidthPercentage = pincerWidthPercentage;
        }
    }
}
