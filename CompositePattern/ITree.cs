using System.Collections.Generic;

namespace CompositePattern
{
    public interface ITree<T>
    {
        IEnumerable<ITree<T>> ThisOrChildren { get; }

        string Id { get; set; }

        T Type { get; set; }

        IEnumerable<string> PerformTask(Pos executor, string task);
    }
}