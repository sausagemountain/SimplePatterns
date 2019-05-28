using System.Collections.Generic;

namespace CompositePattern
{
    public class Employee: ITree<Pos>
    {
        protected Employee()
        {
            ThisOrChildren = new[] { this };
        }
        public Employee(Pos type) : this(type, Program.Rnd.Next().ToString()) { }
        public Employee(Pos type, string id): this()
        {
            Id = id;
            Type = type;
        }

        public IEnumerable<ITree<Pos>> ThisOrChildren { get; }

        public string Id { get; set; } = string.Empty;
        public Pos Type { get; set; }

        public IEnumerable<string> PerformTask(Pos executor, string task)
        {
            if (Type != executor)
                return new string[0];

            return new[] { $"Task '{task}' completed by {Type} {Id}" };
        }

        public override string ToString()
        {
            return $"{Type}: {Id}";
        }
    }
}