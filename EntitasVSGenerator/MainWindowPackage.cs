using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;
using EntitasVSGenerator.Extensions;
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
    sealed class MainWindowPackage : Package
    {
        public const string PackageGuidString = "9f4d054c-8bcc-4a90-ab81-3e1d00ff8a08";

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
            var runningDocumentTable = new RunningDocumentTable(this);
            var vsFileChangeEx = (IVsFileChangeEx)GetService(typeof(SVsFileChangeEx));

            try
            {
                // edge case: no projects in solution
                // edge case: user deletes project after ConfigFile instantiation
                string solutionDirectory = dte.Solution.GetDirectory();
                var configFile = new ConfigFile(solutionDirectory);
                var logicController = new LogicController(configFile, dte, runningDocumentTable, vsFileChangeEx);
                logicController.Run();

                MainWindowCommand.Initialize(this, logicController.Model);
            }
            catch (Exception e)
            {
                File.WriteAllLines($@"{dte.Solution.GetDirectory()}\entitas-vs.log", new[] { e.ToString() });
                throw;
            }
        }
    }
}
