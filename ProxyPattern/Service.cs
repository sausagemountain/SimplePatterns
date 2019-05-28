using System;
using System.Linq;
using System.Threading;

namespace ProxyPattern
{
    public class Service: IService
    {
        public double CalculateIndex (string name, string position)
        {
            var t = string.Join(" ", (name + " " + position).ToLower()
                                                            .Split(' ', '\t', '\n', '\r')
                                                            .Where(e => !string.IsNullOrWhiteSpace(e))
                                                            .OrderBy(e => e));
            Thread.Sleep(500);
            return Math.Sqrt(Math.Abs(t.GetHashCode() / 11.5d) + 2) - 1;
        }
    }
}