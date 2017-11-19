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
    sealed class MainWindowPackage : Package, IVsPersistSolutionProps
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void OnSolutionLoad()
        {
            base.Initialize();
            // Services
            var dte = (DTE)GetService(typeof(DTE));
            var runningDocumentTable = new RunningDocumentTable(this);
            var vsFileChangeEx = (IVsFileChangeEx)GetService(typeof(SVsFileChangeEx));
            string solutionDirectory = PathUtil.GetSolutionDirectory(dte);

            // ViewModel that contains the paths
            var model = new MainWindowModel(new ConfigureTabModel(LoadPaths(dte)), new OverviewTabModel());
            model.ConfigureModel.Paths.CollectionChanged += (sender, e) => OnPathCollectionChanged(dte, sender, e);

            // Logic
            var reloader = new ProjectReloader(dte, vsFileChangeEx);
            var pathContainer = new PathContainer(model.ConfigureModel.Paths);
            var codeGeneratorInvoker = new CodeGeneratorInvoker($"{solutionDirectory}\\");
            var runGeneratorOnSave = new RunGeneratorOnSave(dte, runningDocumentTable, codeGeneratorInvoker, pathContainer, reloader);
            runningDocumentTable.Advise(runGeneratorOnSave);

            MainWindowCommand.Initialize(this, model);
        }

        private void OnPathCollectionChanged(DTE dte, object sender, NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<string> paths = (IEnumerable<string>)sender;
            SavePaths(dte, paths);
        }

        private string[] LoadPaths(DTE dte)
        {
            string settingsPath = PathUtil.GetSettingsPath(dte);
            if (!File.Exists(settingsPath))
                return null;
            return File.ReadAllLines(settingsPath);
        }

        private void SavePaths(DTE dte, IEnumerable<string> pathsToWrite)
        {
            File.WriteAllText(PathUtil.GetSettingsPath(dte), pathsToWrite.ToDelimitedString("\n"));
        }

        #region IVsPersistSolutionProps interface methods

        public int SaveUserOptions(IVsSolutionPersistence pPersistence)
        {
            return VSConstants.S_OK;
        }

        public int LoadUserOptions(IVsSolutionPersistence pPersistence, uint grfLoadOpts)
        {
            return VSConstants.S_OK;
        }

        public int WriteUserOptions(IStream pOptionsStream, string pszKey)
        {
            return VSConstants.S_OK;
        }

        public int ReadUserOptions(IStream pOptionsStream, string pszKey)
        {
            return VSConstants.S_OK;
        }

        public int QuerySaveSolutionProps(IVsHierarchy pHierarchy, VSQUERYSAVESLNPROPS[] pqsspSave)
        {
            return VSConstants.S_OK;
        }

        public int SaveSolutionProps(IVsHierarchy pHierarchy, IVsSolutionPersistence pPersistence)
        {
            return VSConstants.S_OK;
        }

        public int WriteSolutionProps(IVsHierarchy pHierarchy, string pszKey, IPropertyBag pPropBag)
        {
            return VSConstants.S_OK;
        }

        public int ReadSolutionProps(IVsHierarchy pHierarchy, string pszProjectName, string pszProjectMk, string pszKey, int fPreLoad, IPropertyBag pPropBag)
        {
            return VSConstants.S_OK;
        }

        public int OnProjectLoadFailure(IVsHierarchy pStubHierarchy, string pszProjectName, string pszProjectMk, string pszKey)
        {
            return VSConstants.S_OK;
        }

        #endregion
    }
}
