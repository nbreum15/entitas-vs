using EntitasVSGenerator.Extensions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;

namespace EntitasVSGenerator.Logic
{
    class LogicController
    {
        private readonly ConfigFile _configFile;
        private readonly DTE _dte;
        private readonly RunningDocumentTable _runningDocumentTable;
        private readonly IVsFileChangeEx _vsFileChangeEx;
        public MainWindowModel Model { get; private set; }

        public LogicController(ConfigFile configFile, DTE dte, RunningDocumentTable runningDocumentTable, IVsFileChangeEx vsFileChangeEx)
        {
            _configFile = configFile;
            _dte = dte;
            _runningDocumentTable = runningDocumentTable;
            _vsFileChangeEx = vsFileChangeEx;
        }

        public void Run()
        {
            List<string> triggersAll = new List<string>();
            foreach (Project project in _dte.Solution.Projects)
            {
                string[] triggers = _configFile.GetTriggers(project.GetFileNameOnly());
                triggersAll.AddRange(triggers);
                var reloader = new ProjectReloader(project, _vsFileChangeEx);
                var pathContainer = new PathContainer(triggers, _configFile.ProjectDirectory);
                var codeGeneratorInvoker = new CodeGeneratorInvoker($@"{_configFile.ProjectDirectory}\");
                var runGeneratorOnSave = new GeneratorRunner(_dte, _runningDocumentTable, codeGeneratorInvoker, pathContainer, reloader);
            }

            MainWindowModel model = new MainWindowModel
            {
                ConfigureModel = new ConfigureTabModel(triggersAll),
                OverviewModel = new OverviewTabModel()
            };
            Model = model;
        }
    }
}
