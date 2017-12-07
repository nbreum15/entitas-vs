using System;

namespace EntitasVSGenerator.Logic
{
    interface IPathContainer
    {
        void Add(params string[] paths);
        void Remove(params string[] paths);
    }

    interface IDirectoryChangeListener : IPathContainer
    {
        event Action Changed;
    }
}
