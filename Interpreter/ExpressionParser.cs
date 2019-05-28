using System;
using System.Collections.Generic;
using System.Linq;

namespace Interpreter
{
    public static class ExpressionParser
    {
        public static string AddSpace(this string expression)
        {
            var operators = new[] { "+", "-", "*", "/", "^", "(", ")" };
            var result = expression;
            foreach (string s in operators) {
                result = result.Replace(s, $" {s} ");
            }

            result = result.Replace('.', ',').Trim();
            do {
                result = result.Replace("  ", " ");
            } while (result.Contains("  "));

            return result;
        }

        public static IExpression ParseInfix(string expression, out Dictionary<string, decimal> context) => 
            ParseRpn(LibRpn.InfixToRpn(expression, LibRpn.RpnOptions.None), out context);

        /*
        public static Node<string> ParseExpression(string expression)
        {
            var expr = expression.AddSpace().Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
            var result = new Node<string>(string.Empty);
            var tokens = new List<string>();

            var firstParen = -1;
            var parenCount = 0;
            var lastParen = -1;


            bool done = false;
            for (var i = 0; i < expr.Length && !done; i++) {

                string ending(bool sign = true)
                {
                    done = true;
                    return string.Join(" ", expr.Skip(sign? i + 1: i));
                }

                switch (expr[i]) {
                    case "(": {
                        if (firstParen == -1)
                            firstParen = i;
                        parenCount++;
                        break;
                    }
                    case ")": {
                        if (firstParen == -1)
                            throw new Exception("Unmatched parenthesis found");
                        parenCount--;
                        if (parenCount == 0) {
                            lastParen = i;
                            var parens = string.Join(" ", expr.Skip(firstParen + 1).Take(lastParen - firstParen - 1));
                            tokens.Add(parens);
                            result.Add(ParseExpression(parens));
                            firstParen = -1;
                            lastParen = -1;
                        }
                        break;
                    }
                    case "+": {
                        if (parenCount != 0)
                            continue;
                        tokens.Add(expr[i]);
                        if (result.Value == string.Empty)
                            result.Value = expr[i];
                        else {
                            var newNode = ParseExpression(ending());
                            if (!result.Add(newNode)) {
                                var t = new Node<string>(expr[i]) {
                                    result,
                                    newNode
                                };
                                result = t;
                            }
                        }
                        break;
                    }
                    case "-": {
                        if (parenCount != 0)
                            continue;
                        tokens.Add(expr[i]);
                        if (result.Value == string.Empty)
                            result.Value = expr[i];
                        else {
                            var newNode = ParseExpression(ending());
                            if (!result.Add(newNode)) {
                                var t = new Node<string>(expr[i]) {
                                    result,
                                    newNode
                                };
                                result = t;
                            }

                            if (expr[i + 1] != "(") {
                                result.Right = newNode.Left;
                                newNode.Left = result;
                                result = newNode;
                            }
                        }
                        break;
                    }
                    case "/":
                    case "*": {
                        if (parenCount != 0)
                            continue;
                        tokens.Add(expr[i]);
                        if (result.Value == string.Empty)
                            result.Value = expr[i];
                        else {
                            var newNode = ParseExpression(ending());
                            if (!result.Add(newNode)) {
                                if (result.Value == "+" || result.Value == "-") {
                                    var t = new Node<string>(expr[i]) {
                                        result,
                                        newNode
                                    };
                                    result = t;
                                }
                                else {
                                    var t = new Node<string>(expr[i]) {
                                        result.Right,
                                        newNode
                                    };
                                    result.Right = t;
                                }
                            }
                        }
                        break;
                    }
                    default: {
                        if (parenCount != 0)
                            continue;
                        tokens.Add(expr[i]);
                        result.Add(expr[i]);
                        break;
                    }
                }
            }

            if (result.Value == string.Empty && result.Right == null)
                return result.Left;

            return result;
        }
        */

        public static IExpression ParseRpn(string rpnExpression, out Dictionary<string, decimal> context)
        {
            context = new Dictionary<string, decimal>();
            var stack = new Stack<IExpression>();

            var tokens = rpnExpression.AddSpace().Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
            foreach (var token in tokens) {
                switch (token) {
                    case "+":
                        stack.Push(new AddExpression(right: stack.Pop(), left: stack.Pop()));
                        break;
                    case "-":
                        stack.Push(new SubtractExpression(right: stack.Pop(), left: stack.Pop()));
                        break;
                    case "*":
                        stack.Push(new MultiplyExpression(right: stack.Pop(), left: stack.Pop()));
                        break;
                    case "/":
                        stack.Push(new DivideExpression(right: stack.Pop(), left: stack.Pop()));
                        break;
                    case "^":
                        stack.Push(new PowerExpression(right: stack.Pop(), left: stack.Pop()));
                        break;
                    default:
                        if (decimal.TryParse(token, out var number))
                            stack.Push(new NumberExpression(number));
                        else {
                            stack.Push(new VariableExpression(token));
                            context[token] = 0;
                        }

                        break;
                }
            }

            if (stack.Count == 1)
                return stack.Pop();
            throw new Exception("Cannot parse expression completely");
        }
    }
}