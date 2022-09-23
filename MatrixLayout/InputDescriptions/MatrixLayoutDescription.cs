using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.InputDescriptions
{
    public class MatrixLayoutDescription
    {
        public readonly MatrixBracketsDescription BracketsDescription;
        public readonly MatrixInteriorMarginsDescription InteriorMarginsDescription;

        public MatrixLayoutDescription(MatrixBracketsDescription bracketsDescription,
            MatrixInteriorMarginsDescription interiorMarginsDescription)
        {
            BracketsDescription = bracketsDescription;
            InteriorMarginsDescription = interiorMarginsDescription;
        }
    }
}
