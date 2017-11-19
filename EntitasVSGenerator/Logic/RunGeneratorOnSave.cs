﻿using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace EntitasVSGenerator.Logic
{
    class RunGeneratorOnSave : IVsRunningDocTableEvents3
    {
        private readonly DTE _dte;
        private readonly RunningDocumentTable _runningDocumentTable;
        private readonly ProjectReloader _reloader;
        private readonly CodeGeneratorInvoker _codeGeneratorInvoker;

        public PathContainer PathContainer { get; }

        public RunGeneratorOnSave(DTE dte, RunningDocumentTable runningDocumentTable, CodeGeneratorInvoker codeGeneratorInvoker, PathContainer fileTrigger, ProjectReloader reloader)
        {
            _dte = dte;
            _runningDocumentTable = runningDocumentTable;
            PathContainer = fileTrigger;
            _reloader = reloader;
            _codeGeneratorInvoker = codeGeneratorInvoker;
        }
        
        public int OnAfterSave(uint docCookie)
        {
            Document document = FindDocument(docCookie);

            if (document == null)
                return VSConstants.S_OK;
            
            // checks whether the document is in list of triggers
            if (PathContainer.Contains(document.FullName) || PathContainer.Contains(document.Path))
            {
                _reloader.IgnoreProjectFileChanges();
                string[] generatedFiles = _codeGeneratorInvoker.Generate();
                _reloader.AddItems(generatedFiles);
                _reloader.UnignoreProjectFileChanges();
            }

            return VSConstants.S_OK;
        }

        private Document FindDocument(uint docCookie)
        {
            var documentInfo = _runningDocumentTable.GetDocumentInfo(docCookie);
            var documentPath = documentInfo.Moniker;
            
            return _dte.Documents.Cast<Document>().FirstOrDefault(doc => doc.FullName == documentPath);
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
