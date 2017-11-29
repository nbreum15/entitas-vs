using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EntitasVSGenerator.Extensions;
using EntitasVSGenerator.Logic;
using EnvDTE80;

namespace EntitasVSGenerator
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(MainWindow))]
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasSingleProject)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasMultipleProjects)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    sealed class EntitasVsPackage : Package
    {
        public const string PackageGuidString = "9f4d054c-8bcc-4a90-ab81-3e1d00ff8a08";

        public EntitasVsPackage()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            MainWindowCommand.Initialize(this, null);
            // PackageLoader loads package when a solution is loaded
            PackageLoader loader = new PackageLoader(OnSolutionLoad);
            var vsSolution = (IVsSolution)GetService(typeof(SVsSolution));
            vsSolution.AdviseSolutionEvents(loader, out uint _);
        }

        private void OnSolutionLoad()
        {
            var dte = (DTE)GetService(typeof(DTE));
            var runningDocumentTable = new RunningDocumentTable(this);
            var vsFileChangeEx = (IVsFileChangeEx)GetService(typeof(SVsFileChangeEx));

            string solutionDirectory = dte.Solution.GetDirectory();
            var configFile = new ConfigFile(solutionDirectory);
            var logicController = new LogicController(configFile, dte, runningDocumentTable, vsFileChangeEx);
            logicController.Run();

            MainWindowCommand.Instance.Model = logicController.Model;
        }
    }
}
