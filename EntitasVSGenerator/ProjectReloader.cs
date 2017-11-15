using EntitasVSGenerator.Extensions;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace EntitasVSGenerator
{
    class ProjectReloader
    {
        private DTE _dte;
        private IVsFileChangeEx _vsFileChangeEx;
        private FileSystemWatcher _watcher;
        private HashSet<string> _alreadyLoadedFiles = new HashSet<string>();
        private int _changeCount = 0;

        public ProjectReloader(DTE dte, IVsFileChangeEx vsFileChangeEx)
        {
            _dte = dte;
            _vsFileChangeEx = vsFileChangeEx;
            SetupFileWatchers();
            AddAlreadyLoadedGeneratedFiles();
        }

        public void IgnoreProjectFileChanges()
        {
            _watcher.EnableRaisingEvents = true;
            IgnoreProjectFileChanges(true);
        }

        public void UnignoreProjectFileChanges()
        {
            IgnoreProjectFileChanges(false);
        }

        private Project Project => _dte.Solution.Projects.Item(1); // TODO: change this to a config

        private void SetupFileWatchers()
        {
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(Project.FileName), "*.csproj");
            _watcher.Changed += CsprojChanged;
            _watcher.NotifyFilter = NotifyFilters.Size;
        }

        private void CsprojChanged(object sender, FileSystemEventArgs e)
        {
            if (_changeCount == 1)
            {
                _watcher.EnableRaisingEvents = false;
                string[] filePaths = LoadGeneratedFilePathsFromCsproj();
                string[] filesToLoad = GetFilesToLoad(filePaths);
                SelectPreviousSelectedItems(filesToLoad);
                UnignoreProjectFileChanges();
                _changeCount = 0;
            }
            else
            {
                _changeCount++;
            }
        }

        private string[] LoadGeneratedFilePathsFromCsproj() // TODO: delegate work to other class
        {
            string generatedFolder = @"Assets\Sources\Generated"; // TODO: make this a config

            var xml = new XmlDocument();
            xml.Load(Project.FullName);
            XmlNamespaceManager mgnr = new XmlNamespaceManager(xml.NameTable);
            mgnr.AddNamespace("msbuild", "http://schemas.microsoft.com/developer/msbuild/2003");
            XmlNodeList nodes = xml.SelectNodes("//msbuild:Compile", mgnr);
            List<string> generatedFiles = new List<string>();
            foreach (XmlNode node in nodes)
            {
                string fileName = node.Attributes[0].Value;
                if (fileName.Contains(generatedFolder))
                {
                    generatedFiles.Add($@"{PathUtil.GetSolutionDirectory(_dte)}\{fileName}");
                }
            }
            return generatedFiles.ToArray();
        }

        private string[] GetFilesToLoad(string[] newlyAddedFiles)
        {
            List<string> filesToLoad = new List<string>();
            foreach (string newlyAddedFile in newlyAddedFiles)
            {
                if (!_alreadyLoadedFiles.Contains(newlyAddedFile))
                {
                    filesToLoad.Add(newlyAddedFile);
                    _alreadyLoadedFiles.Add(newlyAddedFile);
                }
            }
            return filesToLoad.ToArray();
        }

        private void AddAlreadyLoadedGeneratedFiles()
        {
            string[] filesFromCsproj = LoadGeneratedFilePathsFromCsproj();
            foreach (string fileFromCsproj in filesFromCsproj)
            {
                _alreadyLoadedFiles.Add(fileFromCsproj);
            }
        }

        private void IgnoreFile(string path, bool shouldIgnore)
        {
            _vsFileChangeEx.IgnoreFile(0, path, shouldIgnore ? 1 : 0);
        }

        private void IgnoreProjectFileChanges(bool shouldIgnore)
        {
            string projectFile = Project.FileName;
            IgnoreFile(projectFile, true);
        }

        public void SelectPreviousSelectedItems(string[] filePaths)
        {
            if (filePaths == null)
                return;

            foreach (string filePath in filePaths)
            {
                Project.ProjectItems.AddFromFile(filePath);
            }
        }
    }
}
