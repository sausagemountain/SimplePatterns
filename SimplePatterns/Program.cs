using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimplePatterns
{
    using static Console;

    class Program
    {
        public static void Main(string[] args)
        {
            IApplicationFactory factory = new IosAppFactory();

            var app = factory.CreateApplication();

            WriteLine(app.GetType());
            WriteLine(app.Style.GetType());
            WriteLine(app.Frontend.GetType());
            WriteLine(app.Backend.GetType());
            WriteLine();

            factory = new AndroidAppFactory();
            app = factory.CreateApplication();

            WriteLine(app.GetType());
            WriteLine(app.Style.GetType());
            WriteLine(app.Frontend.GetType());
            WriteLine(app.Backend.GetType());

            ReadKey(true);
        }
    }
}