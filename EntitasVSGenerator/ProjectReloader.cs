using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;

namespace EntitasVSGenerator
{
    class ProjectReloader
    {
        private IVsSolution _solution;
        private IVsSolution4 _solution4;
        private DTE _dte;
        private IVsFileChangeEx _vsFileChangeEx;

        public ProjectReloader(IVsSolution vsSolution, IVsSolution4 vsSolution4, DTE dte, IVsFileChangeEx vsFileChangeEx)
        {
            _solution = vsSolution;
            _solution4 = vsSolution4;
            _dte = dte;
            _vsFileChangeEx = vsFileChangeEx;
            SetupFileWatchers();
        }

        private Projects Projects => _dte.Solution.Projects;

        private void SetupFileWatchers()
        {
            foreach (Project project in Projects)
            {
                FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(project.FileName));
                watcher.Changed += CsprojChanged;
                watcher.EnableRaisingEvents = true;
            }
        }

        private void CsprojChanged(object sender, FileSystemEventArgs e)
        {
            var activeDocument = _dte.ActiveDocument;
            ReloadAllProjects();
            UnignoreProjectFileChanges();
        }

        private void IgnoreFile(string path, bool shouldIgnore)
        {
            _vsFileChangeEx.IgnoreFile(0, path, shouldIgnore ? 1 : 0);
        }

        private void IgnoreProjectFileChanges(bool shouldIgnore)
        {
            foreach (Project project in Projects)
            {
                
                string projectFile = project.FileName;
                IgnoreFile(projectFile, true);
            }
        }

        public void IgnoreProjectFileChanges()
        {
            IgnoreProjectFileChanges(true);
        }

        public void UnignoreProjectFileChanges()
        {
            IgnoreProjectFileChanges(false);
        }

        private void ReloadAllProjects()
        {
            foreach (Project project in Projects)
            {
                if (_solution.GetProjectOfUniqueName(project.UniqueName, out IVsHierarchy vsHierarchy) == VSConstants.S_OK)
                {
                    if (_solution.GetGuidOfProject(vsHierarchy, out Guid guid) == VSConstants.S_OK)
                    {
                        _solution4.UnloadProject(guid, (uint)_VSProjectUnloadStatus.UNLOADSTATUS_UnloadedByUser);
                        _solution4.ReloadProject(guid);
                    }
                }
            }
        }
    }
}
