using System;
using System.Collections.Generic;

namespace Interpreter
{
    public class PowerExpression: IExpression
    {
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }

        public PowerExpression(IExpression left, IExpression right)
        {
            Right = right;
            Left = left;
        }

        public decimal Interpret(Dictionary<string, decimal> context)
        {
            return (decimal)Math.Pow((double)Left.Interpret(context), (double)Right.Interpret(context));
        }

        public override string ToString()
        {
            return $"({Left} ^ {Right})";
        }
    }
}