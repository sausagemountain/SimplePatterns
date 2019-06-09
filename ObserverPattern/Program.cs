using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverPattern
{
    class Program
    {
        public static readonly Random rnd = new Random();

        static void Main(string[] args)
        {
            var t = new NewsPublisher();
            var all = new List<IObserver>();

            for (int i = 0; i < 15; i++) {
                if (rnd.NextDouble() <= 0.5)
                    all.Add(new Lover(i.ToString()));
                else
                    all.Add(new Hater(i.ToString()));
            }
            all.ForEach(e => t.AddObserver(e));

            t.NotifyObservers();
            Console.WriteLine();

            t.NotifyObservers();
            Console.WriteLine();

            t.NotifyObservers();
            Console.WriteLine();

            t.NotifyObservers();
            Console.WriteLine();

            Console.ReadKey(true);
        }
    }
}
