using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace EntitasVSGenerator.Logic
{
    class ProjectReloader
    {
        private DTE _dte;
        private IVsFileChangeEx _vsFileChangeEx;

        public ProjectReloader(DTE dte, IVsFileChangeEx vsFileChangeEx)
        {
            _dte = dte;
            _vsFileChangeEx = vsFileChangeEx;
        }

        public void IgnoreProjectFileChanges()
        {
            IgnoreProjectFileChanges(true);
        }

        public void UnignoreProjectFileChanges()
        {
            IgnoreProjectFileChanges(false);
        }

        public void AddItems(string[] paths)
        {
            foreach (string path in paths)
            {
                AddItemToProject(path);
            }
        }

        private Project Project => _dte.Solution.Projects.Item(1); // TODO: change this to a config

        private void AddItemToProject(string filePath)
        {
            if (filePath == null)
                return;
            Project.ProjectItems.AddFromFile(filePath);
        }

        private void IgnoreFile(string path, bool shouldIgnore)
        {
            _vsFileChangeEx.IgnoreFile(0, path, shouldIgnore ? 1 : 0);
        }

        private void IgnoreProjectFileChanges(bool shouldIgnore)
        {
            IgnoreFile(Project.FileName, true);
        }
    }
}
