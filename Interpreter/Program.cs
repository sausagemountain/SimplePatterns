using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            do {
                Console.WriteLine("Enter an expression. Empty string exits the app.");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                var expr = ExpressionParser.ParseInfix(input, out var context);

                Console.WriteLine("Your Expression with parenthesis is:");
                Console.WriteLine(expr);
                Console.WriteLine();

                foreach (var key in context.Keys.ToList()) {
                    Console.Write($"Enter variable {key}: ");
                    decimal.TryParse(Console.ReadLine(), out var number);
                    context[key] = number;
                }

                var result = expr.Interpret(context);
                Console.WriteLine(result);
            } while (true);
        }
    }
}
