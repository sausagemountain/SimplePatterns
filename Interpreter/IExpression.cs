using System.Collections.Generic;

namespace Interpreter
{
    public interface IExpression
    {
        decimal Interpret(Dictionary<string, decimal> context);
    }
}