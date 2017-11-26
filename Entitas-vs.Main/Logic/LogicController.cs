using EntitasVSGenerator.Extensions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.Linq;

namespace EntitasVSGenerator.Logic
{
    class LogicController
    {
        private readonly ConfigFile _configFile;
        private readonly DTE _dte;
        private readonly RunningDocumentTable _runningDocumentTable;
        private readonly IVsFileChangeEx _vsFileChangeEx;
        public MainWindowModel Model { get; private set; }
        private bool _generatorLoaded;
        private Dictionary<string, PathContainer> _pathContainers = new Dictionary<string, PathContainer>();

        public LogicController(ConfigFile configFile, DTE dte, RunningDocumentTable runningDocumentTable, IVsFileChangeEx vsFileChangeEx)
        {
            _configFile = configFile;
            _dte = dte;
            _runningDocumentTable = runningDocumentTable;
            _vsFileChangeEx = vsFileChangeEx;
        }

        public void Run()
        {
            var projectItems = GetProjectItems();
            var generatorPath = _configFile.GeneratorPath;
            foreach ((Project project, ProjectItem projectItem) in projectItems)
            {
                projectItem.Changed += ProjectItem_Changed;
            }

            if (generatorPath != null)
            {
                LoadGeneratorLogic(generatorPath, projectItems);
                _generatorLoaded = true;
            }
            else
            {
                _generatorLoaded = false;
            }

            MainWindowModel model = new MainWindowModel
            {
                OverviewTabModel = new OverviewTabModel(),
                ConfigureTabModel = new ConfigureTabModel(
                    projectItems.Select(tuple => tuple.Item2).ToArray(), 
                    generatorPath,
                    _dte.Solution.GetDirectory())
            };
            Model = model;
            Model.ConfigureTabModel.GeneratePathChanged += GeneratePathChanged;
            Model.ConfigureTabModel.GeneratorLoadClick += () => GeneratorLoadClick(projectItems);
            Model.ConfigureTabModel.IsGeneratorLoaded = _generatorLoaded;
        }

        private void GeneratorLoadClick(List<(Project, ProjectItem)> projectItems)
        {
            if (!_generatorLoaded)
            {
                LoadGeneratorLogic(_configFile.GeneratorPath, projectItems);
                _generatorLoaded = true;
            }
        }

        private void GeneratePathChanged(string path)
        {
            _configFile.GeneratorPath = path;
        }

        private void LoadGeneratorLogic(string generatorPath, List<(Project, ProjectItem)> projectItems)
        {
            AssemblyExtensions.CopyDllsToGeneratorDirectory(generatorPath, _dte.Solution.GetDirectory());
            foreach ((Project project, ProjectItem projectItem) in projectItems)
            {
                var reloader = new ProjectReloader(project, _vsFileChangeEx);
                var pathContainer = new PathContainer(projectItem.Triggers, projectItem.Directory);
                _pathContainers.Add(projectItem.Directory, pathContainer);
                var codeGenerator = AssemblyExtensions.GetGenerator(_configFile.GeneratorPath, projectItem.Directory, _dte.Solution.GetDirectory());
                var runGeneratorOnSave = new GeneratorRunner(_dte, _runningDocumentTable, codeGenerator, pathContainer, reloader, project);
            }
        }

        private void ProjectItem_Changed(ProjectItem item)
        {
            _configFile.Refresh(item);
            _pathContainers[item.Directory].Triggers = item.Triggers;
        }

        private List<(Project, ProjectItem)> GetProjectItems()
        {
            var projectItems = new List<(Project, ProjectItem)>();
            foreach (Project project in _dte.Solution.Projects)
            {
                var triggers = _configFile.GetTriggers(project.GetFileNameOnly());
                ProjectItem item = new ProjectItem(project.GetFileNameOnly(), triggers.ToList(), project.GetDirectory());
                projectItems.Add((project, item));
            }
            return projectItems;
        }
    }
}
