﻿using EntitasVSGenerator.Extensions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.Collections.Specialized;
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

            foreach ((Project project, ProjectItem projectItem) in projectItems)
            {
                projectItem.Changed += ProjectItem_Changed;
                var reloader = new ProjectReloader(project, _vsFileChangeEx);
                var pathContainer = new PathContainer(projectItem.Triggers, project.GetDirectory());
                var codeGeneratorInvoker = new CodeGeneratorInvoker(project.GetDirectory());
                var runGeneratorOnSave = new GeneratorRunner(_dte, _runningDocumentTable, codeGeneratorInvoker, pathContainer, reloader, project);
            }

            MainWindowModel model = new MainWindowModel
            {
                OverviewTabModel = new OverviewTabModel(),
                ConfigureTabModel = new ConfigureTabModel(projectItems.Select(tuple => tuple.Item2).ToArray())
            };
            Model = model;
        }

        private void ProjectItem_Changed(ProjectItem item, string oldProjectName)
        {
            _configFile.Refresh(item, oldProjectName);
        }

        private List<(Project, ProjectItem)> GetProjectItems()
        {
            var projectItems = new List<(Project, ProjectItem)>();
            foreach (Project project in _dte.Solution.Projects)
            {
                var triggers = _configFile.GetTriggers(project.GetFileNameOnly());
                ProjectItem item = new ProjectItem(project.GetFileNameOnly(), triggers.ToList());
                projectItems.Add((project, item)); // valuetuple
            }
            return projectItems;
        }
    }
}