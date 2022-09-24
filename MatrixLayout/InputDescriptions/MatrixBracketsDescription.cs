using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.InputDescriptions
{
    public class MatrixBracketsDescription
    {
        public readonly float Thickness;
        public readonly float PincerWidth;

        public MatrixBracketsDescription(float thickness, float pincerWidth)
        {
            Thickness = thickness;
            PincerWidth = pincerWidth;
        }
    }
}
