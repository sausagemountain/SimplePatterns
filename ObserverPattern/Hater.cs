using System;

namespace ObserverPattern
{
    class Hater: IObserver
    {
        public Hater(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; } = string.Empty;

        public void Update()
        {
            Console.Write($"{Name} receives news and ");

            if (Program.rnd.NextDouble() <= 0.6)
                Console.WriteLine("hates it.");
            else
                Console.WriteLine("wants to unsubscribe.");
        }
    }
}