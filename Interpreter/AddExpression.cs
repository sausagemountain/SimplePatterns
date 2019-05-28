﻿using System.Collections.Generic;

namespace Interpreter
{
    public class AddExpression: IExpression
    {
        public IExpression Left { get; set; }
        public IExpression Right { get; set; }

        public AddExpression(IExpression left, IExpression right)
        {
            Right = right;
            Left = left;
        }

        public decimal Interpret(Dictionary<string, decimal> context)
        {
            return Left.Interpret(context) + Right.Interpret(context);
        }

        public override string ToString()
        {
            return $"({Left} + {Right})";
        }
    }
}