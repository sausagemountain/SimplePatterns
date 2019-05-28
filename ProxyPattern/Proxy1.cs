using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyPattern
{
    public class Proxy1: IService
    {
        public Proxy1 () { }
        public Proxy1 (IService realService): this() { this.Service = realService; }

        public IService Service { get; set; }

        public bool IsAccessible { get; protected set; } = true;

        public Dictionary<(string, string), double> results = new Dictionary<(string, string), double>();

        public double CalculateIndex (string name, string position)
        {
            var result = 0d;
            if (!results.TryGetValue((name, position), out result))
                if (IsAccessible) {
                    results.Add((name, position), result = Service.CalculateIndex(name, position));
                }
                else
                    throw new Exception("Cannot Access service");

            return result;
        }
    }
}