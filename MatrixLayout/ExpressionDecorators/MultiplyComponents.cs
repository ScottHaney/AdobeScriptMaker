﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class MultiplyComponents : IAddableComponent, INumericMultiplierCapableComponent, IExpressionComponent
    {
        public readonly ILeftMultipliableComponent Lhs;
        public readonly IRightMultipliableComponent Rhs;

        public MultiplyComponents(ILeftMultipliableComponent lhs, IRightMultipliableComponent rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }
    }

    public interface ILeftMultipliableComponent
    { }

    public interface IRightMultipliableComponent
    { }

    public interface IMultipliableComponent : ILeftMultipliableComponent, IRightMultipliableComponent
    { }
}
