using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;
using System.Collections.Generic;
using MoreLinq;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio;
using System.Collections.Specialized;
using EntitasVSGenerator.Extensions;
using Entitas.CodeGeneration.CodeGenerator;
using EntitasVSGenerator.Logic;

namespace EntitasVSGenerator
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(MainWindow))]
    [Guid(MainWindowPackage.PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasSingleProject)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasMultipleProjects)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    sealed class MainWindowPackage : Package//, IVsPersistSolutionProps
    {
        public const string PackageGuidString = "9f4d054c-8bcc-4a90-ab81-3e1d00ff8a08";
        public static Project CurrentProject { get; private set; }
        public string ProjectDirectory => PathUtil.GetProjectDirectory(CurrentProject);

        public MainWindowPackage()
        {
        }

        protected override void Initialize()
        {
            // PackageLoader loads package when a solution is loaded
            PackageLoader loader = new PackageLoader(OnSolutionLoad);
            var vsSolution = (IVsSolution)GetService(typeof(SVsSolution));
            vsSolution.AdviseSolutionEvents(loader, out uint _);
        }

        private void OnSolutionLoad()
        {
            base.Initialize();
            var dte = (DTE)GetService(typeof(DTE));
            SelectProject(dte);
            var runningDocumentTable = new RunningDocumentTable(this);
            var vsFileChangeEx = (IVsFileChangeEx)GetService(typeof(SVsFileChangeEx));

            try
            {
                // ViewModel that contains the paths
                var model = new MainWindowModel(new ConfigureTabModel(LoadPaths(ProjectDirectory)), new OverviewTabModel());
                model.ConfigureModel.Paths.CollectionChanged += (sender, e) => OnPathCollectionChanged(ProjectDirectory, sender, e);

                // Logic
                var reloader = new ProjectReloader(CurrentProject, vsFileChangeEx);
                var pathContainer = new PathContainer(model.ConfigureModel.Paths, ProjectDirectory);
                var codeGeneratorInvoker = new CodeGeneratorInvoker($@"{ProjectDirectory}\");
                var runGeneratorOnSave = new RunGeneratorOnSave(dte, runningDocumentTable, codeGeneratorInvoker, pathContainer, reloader);
                runningDocumentTable.Advise(runGeneratorOnSave);

                MainWindowCommand.Initialize(this, model);
            }
            catch (Exception e)
            {
                File.WriteAllLines($@"{ProjectDirectory}\entitas-vs.log", new[] { e.ToString() });
                throw;
            }
        }

        private void OnPathCollectionChanged(string projectDirectory, object sender, NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<string> paths = (IEnumerable<string>)sender;
            SavePaths(projectDirectory, paths);
        }

        private string[] LoadPaths(string projectDirectory)
        {
            string settingsPath = PathUtil.GetSettingsPath(projectDirectory);
            if (!File.Exists(settingsPath))
                return null;
            return File.ReadAllLines(settingsPath);
        }

        private void SavePaths(string projectDirectory, IEnumerable<string> pathsToWrite)
        {
            File.WriteAllText(PathUtil.GetSettingsPath(projectDirectory), pathsToWrite.ToDelimitedString("\n"));
        }

        private void SelectProject(DTE dte)
        {
            CurrentProject = dte.Solution.Projects.Item(1);
        }
    }
}
