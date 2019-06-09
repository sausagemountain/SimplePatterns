using System;

namespace ObserverPattern
{
    class Lover: IObserver
    {
        public Lover(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; } = string.Empty;

        public void Update()
        {
            Console.Write($"{Name} receives news");
            if (Program.rnd.NextDouble() <= 0.5)
                Console.WriteLine($", reads it.");
            else
                Console.WriteLine($".");
        }
    }
}