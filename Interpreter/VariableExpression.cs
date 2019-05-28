using System.Collections.Generic;

namespace Interpreter
{
    public class VariableExpression: IExpression
    {
        public string Name { get; }

        public VariableExpression(string name)
        {
            Name = name;
        }

        public decimal Interpret(Dictionary<string, decimal> context)
        {
            context.TryGetValue(Name, out var result);
            return result;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}