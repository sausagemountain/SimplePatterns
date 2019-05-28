using System.Collections.Generic;

namespace Interpreter
{
    public class NumberExpression: IExpression
    {
        private decimal Number { get; }

        public NumberExpression(decimal number)
        {
            Number = number;
        }

        public decimal Interpret(Dictionary<string, decimal> context)
        {
            return Number;
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}