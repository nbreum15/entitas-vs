using System;

namespace Entitas_vs.Main.Logic
{
    interface IDirectoryChangeListener : IPathContainer
    {
        event Action Changed;
        void RaiseEvent();
        void ClearEventListeners();
    }
}
