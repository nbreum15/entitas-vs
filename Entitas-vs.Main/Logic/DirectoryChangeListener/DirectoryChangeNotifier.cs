using System.Collections.Generic;
using Entitas_vs.Common;
using Entitas_vs.Main.Extensions;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Entitas_vs.Main.Logic
{
    class DirectoryChangeNotifier : IDirectoryChangeNotifier, IVsRunningDocTableEvents
    {
        private readonly IVsRunningDocumentTable _runningDocumentTable;
        private readonly List<IDirectoryChangeListener> _directoryChangeListeners;

        public DirectoryChangeNotifier(IVsRunningDocumentTable runningDocumentTable)
        {
            _runningDocumentTable = runningDocumentTable;
            _directoryChangeListeners = new List<IDirectoryChangeListener>();
            _runningDocumentTable.AdviseRunningDocTableEvents(this, out _);
        }

        public void AddListener(IDirectoryChangeListener listener)
        {
            _directoryChangeListeners.Add(listener);
        }

        public void RemoveListener(IDirectoryChangeListener listener)
        {
            _directoryChangeListeners.Remove(listener);
        }

        public void ClearListeners()
        {
            foreach (var listener in _directoryChangeListeners)
            {
                listener.ClearEventListeners();
            }
            _directoryChangeListeners.Clear();
        }

        public int OnAfterSave(uint docCookie)
        {
            _runningDocumentTable.GetDocumentInfo(docCookie, out _, out _, out _, out string filePath, out _, out _, out _);
            foreach (var directoryChangeListener in _directoryChangeListeners)
            {
                foreach (var folderPath in directoryChangeListener.Paths)
                {
                    if (folderPath.IsSubpathOf(filePath.ToLower()))
                    {
                        directoryChangeListener.RaiseEvent();
                        break;
                    }
                }
            }
            return VSConstants.S_OK;
        }

        #region Other IVsRunningDocTableEvents3 inferface methods
        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld, string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeSave(uint docCookie)
        {
            return VSConstants.S_OK;
        }
        #endregion
    }
}
