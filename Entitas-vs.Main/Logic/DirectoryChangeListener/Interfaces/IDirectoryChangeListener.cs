using System;

namespace EntitasVSGenerator.Logic
{
    interface IDirectoryChangeListener : IPathContainer
    {
        event Action Changed;
        void RaiseEvent();
        void ClearEventListeners();
    }
}
