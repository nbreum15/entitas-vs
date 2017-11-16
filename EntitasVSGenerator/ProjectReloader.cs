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
        private FileSystemWatcher _genFolderWatcher;
        private FileSystemWatcher _csprojWatcher;
        private HashSet<string> _alreadyLoadedFiles = new HashSet<string>();

        public ProjectReloader(DTE dte, IVsFileChangeEx vsFileChangeEx)
        {
            _dte = dte;
            _vsFileChangeEx = vsFileChangeEx;
            SetupGeneratedFolderWatcher();
            SetupCsprojWatcher();
            AddAlreadyLoadedGeneratedFiles();
        }

        public void IgnoreProjectFileChanges()
        {
            _genFolderWatcher.EnableRaisingEvents = true;
            _csprojWatcher.EnableRaisingEvents = true;
            IgnoreProjectFileChanges(true);
        }

        public void UnignoreProjectFileChanges()
        {
            _csprojWatcher.EnableRaisingEvents = false;
            IgnoreProjectFileChanges(false);
        }

        private void AddItemToProject(string filePath)
        {
            if (filePath == null)
                return;
            Project.ProjectItems.AddFromFile(filePath);
        }

        private Project Project => _dte.Solution.Projects.Item(1); // TODO: change this to a config

        private void SetupGeneratedFolderWatcher()
        {
            _genFolderWatcher = new FileSystemWatcher(Path.GetDirectoryName(Project.FileName) + "\\" + @"Assets\Sources\Generated", "*.cs");
            _genFolderWatcher.IncludeSubdirectories = true;
            _genFolderWatcher.Changed += GeneratedFilesChanged;
            _genFolderWatcher.NotifyFilter = NotifyFilters.Size;
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
            if (!_alreadyLoadedFiles.Contains(e.FullPath))
            {
                AddItemToProject(e.FullPath);
            }
        }

        private void IgnoreFile(string path, bool shouldIgnore)
        {
            _vsFileChangeEx.IgnoreFile(0, path, shouldIgnore ? 1 : 0);
        }

        private void IgnoreProjectFileChanges(bool shouldIgnore)
        {
            IgnoreFile(Project.FileName, true);
        }

        private void AddAlreadyLoadedGeneratedFiles()
        {
            string[] filesFromCsproj = LoadGeneratedFilePathsFromCsproj();
            foreach (string fileFromCsproj in filesFromCsproj)
            {
                _alreadyLoadedFiles.Add(fileFromCsproj);
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
    }
}
