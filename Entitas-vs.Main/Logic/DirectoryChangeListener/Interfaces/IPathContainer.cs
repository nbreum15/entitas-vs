using System.Collections.Generic;

namespace EntitasVSGenerator.Logic
{
    interface IPathContainer
    {
        void Add(params string[] paths);
        void Remove(params string[] paths);
        IEnumerable<string> Paths { get; }
    }
}