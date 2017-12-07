using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;

namespace EntitasVSGenerator.Logic
{
    class DirectoryChangeListener : IVsFileChangeEvents, IDisposable, IDirectoryChangeListener
    {
        public event Action Changed;

        private readonly IVsFileChangeEx _vsFileChangeEx;
        private readonly Dictionary<string, uint> _cookies = new Dictionary<string, uint>();

        public DirectoryChangeListener(IVsFileChangeEx vsFileChangeEx)
        {
            _vsFileChangeEx = vsFileChangeEx;
        }

        public virtual void Add(params string[] paths)
        {
            foreach (string path in paths)
            {
                _vsFileChangeEx.AdviseDirChange(path, 1, this, out uint cookie);
                if (!_cookies.ContainsKey(path))
                    _cookies.Add(path, cookie);
            }
        }

        public virtual void Remove(params string[] paths)
        {
            foreach (string path in paths)
            {
                if(_cookies.TryGetValue(path, out uint cookie))
                    _vsFileChangeEx.UnadviseDirChange(cookie);
            }
        }

        public int DirectoryChanged(string pszDirectory)
        {
            OnChanged();
            return VSConstants.S_OK;
        }

        public int FilesChanged(uint cChanges, string[] rgpszFile, uint[] rggrfChange)
        {
            return VSConstants.S_OK;
        }

        public void Dispose()
        {
            foreach (var cookie in _cookies.Values)
            {
                _vsFileChangeEx.UnadviseDirChange(cookie);
            }
        }

        protected void OnChanged()
        {
            Changed?.Invoke();
        }
    }
}
