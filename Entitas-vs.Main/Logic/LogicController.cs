using EntitasVSGenerator.Extensions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MoreLinq;
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
        private readonly Dictionary<string, PathContainer> _pathContainers = new Dictionary<string, PathContainer>();

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
            foreach ((Project project, ProjectViewModel projectItem) in projectItems)
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
        
        private void LoadGeneratorLogic(string generatorPath, List<(Project, ProjectViewModel)> projectItems)
        {
            AssemblyExtensions.CopyDllsToGeneratorDirectory(generatorPath, _dte.Solution.GetDirectory());
            foreach ((Project project, ProjectViewModel projectItem) in projectItems)
            {
                var pathContainer = new PathContainer(projectItem.Triggers, projectItem.Directory);
                _pathContainers.Add(projectItem.Directory, pathContainer);
                var codeGenerator = AssemblyExtensions.GetGenerator(_configFile.GeneratorPath, projectItem.Directory, _dte.Solution.GetDirectory());
                var runGeneratorOnSave = new GeneratorRunner(_dte, _runningDocumentTable, codeGenerator, pathContainer, project);
            }
        }

        private List<(Project, ProjectViewModel)> GetProjectItems()
        {
            var projectItems = new List<(Project, ProjectViewModel)>();
            foreach (Project project in _dte.Solution.GetAllProjects())
            {
                var triggers = _configFile.GetTriggers(project.GetFileNameOnly());
                ProjectViewModel viewModel = new ProjectViewModel(project.GetFileNameOnly(), triggers.ToList(), project.GetDirectory());
                projectItems.Add((project, viewModel));
            }
            return projectItems;
        }

        #region UI Events
        private void GeneratorLoadClick(List<(Project, ProjectViewModel)> projectItems)
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

        private void ProjectItem_Changed(ProjectViewModel viewModel)
        {
            _configFile.Refresh(viewModel);
            _pathContainers[viewModel.Directory].Triggers = viewModel.Triggers;
        }
        #endregion
    }
}
