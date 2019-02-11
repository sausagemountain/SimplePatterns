using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ServerDirector(new BaselineServerBuilder());
            var server = builder.Construct();
            Console.WriteLine(server);

            builder.ServerBuilder = new HighEndServerBuilder();
            server = builder.Construct();
            Console.WriteLine(server);

            builder.ServerBuilder = new MidRangeServerBuilder();
            server = builder.Construct();
            Console.WriteLine(server);

            Console.ReadKey(true);
        }
    }
}
