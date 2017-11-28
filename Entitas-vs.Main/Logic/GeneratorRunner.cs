using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Entitas_vs.Contract;
using EntitasVSGenerator.Extensions;
using Task = System.Threading.Tasks.Task;
using System.Threading.Tasks;

namespace EntitasVSGenerator.Logic
{
    class GeneratorRunner : IVsRunningDocTableEvents3
    {
        private readonly DTE _dte;
        private readonly RunningDocumentTable _runningDocumentTable;
        private readonly Project _project;
        private readonly IGenerator _codeGenerator;
        private readonly PathContainer _pathContainer;
        private string[] _oldGeneratedFiles;

        public GeneratorRunner(DTE dte, 
            RunningDocumentTable runningDocumentTable, 
            IGenerator codeGenerator, 
            PathContainer fileTrigger,
            Project project)
        {
            _dte = dte;
            _runningDocumentTable = runningDocumentTable;
            _pathContainer = fileTrigger;
            _project = project;
            _codeGenerator = codeGenerator;
            _runningDocumentTable.Advise(this);
        }
        
        public int OnAfterSave(uint docCookie)
        {
            Document document = FindDocument(docCookie);
            
            if (document == null || document.ProjectItem.ContainingProject != _project)
                return VSConstants.S_OK;

            if (_pathContainer.Contains(document.FullName))
            {
                Task.Run(() =>
                {
                    string[] generatedFiles = _codeGenerator.Generate();
                    string[] deletedFiles = GetDeletedGeneratedFiles(generatedFiles);
                    RemoveItems(deletedFiles);
                    AddItems(generatedFiles);
                });
            }

            return VSConstants.S_OK;
        }

        private string[] GetDeletedGeneratedFiles(string[] newGeneratedFiles)
        {
            if (_oldGeneratedFiles == null || newGeneratedFiles == null)
                return new string[0];
            return _oldGeneratedFiles.GetDeletedFileNames(newGeneratedFiles).ToArray();
        }

        private void RemoveItems(string[] deletedFileNames)
        {
            if (deletedFileNames == null)
                return;
            _dte.Solution.RemoveItems(deletedFileNames);
        }

        private void AddItems(string[] generatedFiles)
        {
            if (generatedFiles == null)
                return;
            _oldGeneratedFiles = generatedFiles;
            _project.AddItems(generatedFiles);
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
