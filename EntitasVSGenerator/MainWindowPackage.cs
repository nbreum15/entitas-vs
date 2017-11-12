using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using MoreLinq;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio;

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
        
        private string SettingsName => "entitas-vs.cfg";

        private InvokeShellCommand _invokeShellCommand;

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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _invokeShellCommand.StopServer();
        }

        private void OnSolutionLoad()
        {
            base.Initialize();
            var dte = (DTE)GetService(typeof(DTE));
            var runningDocumentTable = new RunningDocumentTable(this);

            // ViewModel that contains the paths
            var model = new MainWindowModel(LoadPaths(dte));
            model.Paths.CollectionChanged += (sender, e) => OnPathCollectionChanged(dte, sender, e);

            PathContainer fileTrigger = new PathContainer(model.Paths);
            _invokeShellCommand = new InvokeShellCommand(GetSolutionDirectory(dte));
            _invokeShellCommand.StartServer();
            var runGeneratorOnSave = new RunGeneratorOnSave(dte, runningDocumentTable, fileTrigger, _invokeShellCommand);
            runningDocumentTable.Advise(runGeneratorOnSave);

            MainWindowCommand.Initialize(this, model);
        }

        private void OnPathCollectionChanged(DTE dte, object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<string> paths = (IEnumerable<string>)sender;
            SavePaths(dte, paths);
        }

        private string GetSettingsPath(DTE dte) => $@"{GetSolutionDirectory(dte)}\{SettingsName}";

        private string GetSolutionDirectory(DTE dte)
        {
#if DEBUG
            return @"C:\Users\nickl\Desktop\entitas-test";
#else
            return dte.Solution.FullName;
#endif
        }

        private string[] LoadPaths(DTE dte)
        {
            string settingsPath = GetSettingsPath(dte);
            if (!File.Exists(settingsPath))
                return null;
            return File.ReadAllLines(settingsPath);
        }

        private void SavePaths(DTE dte, IEnumerable<string> pathsToWrite)
        {
            File.WriteAllText(GetSettingsPath(dte), pathsToWrite.ToDelimitedString("\n"));
        }

        {

        }
    }
}
