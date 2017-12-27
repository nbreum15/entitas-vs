using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Entitas_vs.Main.Logic
{
    class DirectoryChangeListener : IDirectoryChangeListener
    {
        public event Action Changed;

        private readonly List<string> _paths = new List<string>();

        public virtual void Add(params string[] paths)
        {
            _paths.AddRange(paths.Select(path => path.ToLower()));
        }

        public virtual void Remove(params string[] paths)
        {
            paths.ForEach(path => _paths.Remove(path));
        }

        public IEnumerable<string> Paths => _paths;

        public void RaiseEvent()
        {
            OnChanged();
        }

        public void ClearEventListeners()
        {
            Changed = null;
            _paths.Clear();
        }

        protected void OnChanged()
        {
            Changed?.Invoke();
        }
    }
}
