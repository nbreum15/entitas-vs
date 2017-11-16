using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;

namespace EntitasVSGenerator
{
    class ProjectReloader
    {
        private DTE _dte;
        private IVsFileChangeEx _vsFileChangeEx;
        private FileSystemWatcher _genFolderWatcher;
        private FileSystemWatcher _csprojWatcher;

        public ProjectReloader(DTE dte, IVsFileChangeEx vsFileChangeEx)
        {
            _dte = dte;
            _vsFileChangeEx = vsFileChangeEx;
            SetupGeneratedFolderWatcher();
            SetupCsprojWatcher();
        }

        public void IgnoreProjectFileChanges()
        {
            _csprojWatcher.EnableRaisingEvents = true;
            IgnoreProjectFileChanges(true);
        }

        public void UnignoreProjectFileChanges()
        {
            _csprojWatcher.EnableRaisingEvents = false;
            IgnoreProjectFileChanges(false);
        }

        private Project Project => _dte.Solution.Projects.Item(1); // TODO: change this to a config

        private void SetupGeneratedFolderWatcher()
        {
            _genFolderWatcher = new FileSystemWatcher(Path.GetDirectoryName(Project.FileName) + "\\" + @"Assets\Sources\Generated", "*.cs");
            _genFolderWatcher.IncludeSubdirectories = true;
            _genFolderWatcher.EnableRaisingEvents = true;
            _genFolderWatcher.Created += GeneratedFilesChanged;
            _genFolderWatcher.NotifyFilter = NotifyFilters.Size; // find better filter
        }

        private void SetupCsprojWatcher()
        {
            _csprojWatcher = new FileSystemWatcher(Path.GetDirectoryName(Project.FileName), "*.csproj");
            _csprojWatcher.Changed += CsprojChanged;
            _csprojWatcher.NotifyFilter = NotifyFilters.Size;
        }

        private void CsprojChanged(object sender, FileSystemEventArgs e)
        {
            UnignoreProjectFileChanges();
        }

        private void GeneratedFilesChanged(object sender, FileSystemEventArgs e)
        {
            AddItemToProject(e.FullPath);
        }

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
