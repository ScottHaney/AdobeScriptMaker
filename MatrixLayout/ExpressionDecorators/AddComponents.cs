using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLayout.ExpressionDecorators
{
    public class AddComponents : IAddableComponent, IExpressionComponent
    {
        public readonly IAddableComponent Lhs;
        public readonly IAddableComponent Rhs;

        public AddComponents(IAddableComponent lhs, IAddableComponent rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public static IExpressionComponent Create(params IAddableComponent[] components)
        {
            if (components.Length < 2)
                throw new NotSupportedException();

            var result = new AddComponents(components[0], components[1]);
            for (int i = 2; i < components.Length; i++)
                result = new AddComponents(result, components[i]);

            return result;
        }
    }

    public interface IAddableComponent : IExpressionComponent
    {

    }
}
