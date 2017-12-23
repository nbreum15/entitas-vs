using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace Entitas_vs.Main.Logic
{
    class PackageLoader : IVsSolutionEvents, IPackageLoader, IDisposable
    {
        public event Action AfterOpenSolution;

        private bool _solutionOpened;
        private readonly uint _cookie;
        private readonly IVsSolution _vsSolution;

        public PackageLoader(IVsSolution vsSolution)
        {
            vsSolution.AdviseSolutionEvents(this, out _cookie);
            _vsSolution = vsSolution;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            if (_solutionOpened)
                return VSConstants.S_OK;
            OnAfterSolutionLoad();
            _solutionOpened = true;
            return VSConstants.S_OK;
        }

        protected void OnAfterSolutionLoad()
        {
            AfterOpenSolution?.Invoke();
        }

        public void Dispose()
        {
            _vsSolution.UnadviseSolutionEvents(_cookie);
        }

        #region Other IVsSolutionEvents inferface methods
        public int OnAfterCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }
        #endregion
    }
}
