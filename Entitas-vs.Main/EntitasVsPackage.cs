using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using EntitasVSGenerator.Extensions;
using EntitasVSGenerator.Logic;
using System.Linq;
using EntitasVSGenerator.ViewLogic.Commands;
using EntitasVSGenerator.ViewLogic.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EntitasVSGenerator
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasSingleProject)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasMultipleProjects)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    sealed class EntitasVsPackage : Package
    {
        public const string PackageGuidString = "9f4d054c-8bcc-4a90-ab81-3e1d00ff8a08";

        /// <summary>
        /// Gets the running document table for the package.
        /// </summary>
        public static RunningDocumentTable RunningDocumentTable { get; private set; }

        /// <summary>
        /// Gets the top extensibility object for the package.
        /// </summary>
        public static DTE DTE { get; private set; }

        /// <summary>
        /// Gets the currently running solution.
        /// </summary>
        public static Solution Solution => DTE.Solution;

        /// <summary>
        /// Gets the file change event handler.
        /// </summary>
        public static IVsFileChangeEx VsFileChangeEx { get; private set; }

        /// <summary>
        /// Gets the solution manager object.
        /// </summary>
        public static IVsSolution VsSolution { get; private set; }

        /// <summary>
        /// Gets the config file object that holds all config functionality.
        /// </summary>
        public static ConfigFile ConfigFile { get; set; }

        public EntitasVsPackage()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Get and set all services
            DTE = (DTE)GetService(typeof(SDTE));
            RunningDocumentTable = new RunningDocumentTable(this);
            VsFileChangeEx = (IVsFileChangeEx)GetService(typeof(SVsFileChangeEx));
            VsSolution = (IVsSolution)GetService(typeof(SVsSolution));

            var packageLoader = FactoryMethods.GetPackageLoader();
            packageLoader.AfterOpenSolution += PackageLoader_AfterOpenSolution;

            SettingsWindowCommand.Initialize(this);
            
        }

        private void PackageLoader_AfterOpenSolution()
        {
            ConfigFile = new ConfigFile(DTE.Solution.GetDirectory());
            ConfigFile.Load();
            //AssemblyExtensions.CopyDllsToGeneratorDirectory(ConfigFile.GeneratorPath, DTE.Solution.GetDirectory());
            
            //foreach (Project project in DTE.Solution.GetAllProjects())
            //{
            //    string[] triggers = ConfigFile.GetTriggers(project.UniqueName);
            //    if (triggers.Length == 0)
            //        continue;

            //    var directoryChangeListener = FactoryMethods.GetRelativeDirectoryChangeListener(project.GetDirectory());
            //    var generatorRunner = new GeneratorRunner(ConfigFile.GeneratorPath, project, DTE.Solution);
            //    directoryChangeListener.Add(triggers);
            //    directoryChangeListener.Changed += () => generatorRunner.Run();
            //}
        }
    }
}
