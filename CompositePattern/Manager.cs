using System;
using System.Collections.Generic;
using System.Linq;

namespace CompositePattern
{
    public class Manager: ITree<Pos>
    {
        public Manager(Pos type): this(type, Program.Rnd.Next().ToString()) { }
        public Manager(Pos type, string id)
        {
            Id = id;
            Type = type;
        }
        public Manager(Pos type, params ITree<Pos>[] selfOrChildren): this(type, Program.Rnd.Next().ToString(), selfOrChildren) { }
        public Manager(Pos type, string id, params ITree<Pos>[] selfOrChildren): this(type, id)
        {
            ThisOrChildren = new List<ITree<Pos>>(selfOrChildren);
        }

        public IEnumerable<ITree<Pos>> ThisOrChildren { get; protected set; } = new List<ITree<Pos>>();

        public string Id { get; set; } = string.Empty;
        public Pos Type { get; set; }

        public IEnumerable<string> PerformTask(Pos executor, string task)
        {
            var result = new List<string>(0);

            if (Type == executor)
                result.Add($"Task '{task}' completed by {Id}");

            foreach (var child in ThisOrChildren) {
                if(child.Type == executor || child.ThisOrChildren.First() != child)
                    result.AddRange(child.PerformTask(executor, task));
            }

            return result;
        }

        public override string ToString()
        {
            var res = $"{Type}: {Id}";
            foreach (var child in ThisOrChildren) {
                res += $"{Environment.NewLine}{child}"
                    .Replace(Environment.NewLine, $"{Environment.NewLine}-");
            }

            return res;
        }
    }
}