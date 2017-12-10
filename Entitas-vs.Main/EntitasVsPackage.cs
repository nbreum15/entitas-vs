using System.Collections.Generic;
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
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasSingleProject)]
    [ProvideAutoLoad(UIContextGuids80.SolutionHasMultipleProjects)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    sealed class EntitasVsPackage : Package
    {
        private bool _dllsCopied = false;
        private IDirectoryChangeNotifier _directoryChangeNotifier;

        public const string PackageGuidString = "9f4d054c-8bcc-4a90-ab81-3e1d00ff8a08";

        /// <summary>
        /// Gets the running document table for the package.
        /// </summary>
        public static IVsRunningDocumentTable RunningDocumentTable { get; private set; }

        /// <summary>
        /// Gets the top extensibility object for the package.
        /// </summary>
        public static DTE2 DTE { get; private set; }

        /// <summary>
        /// Gets the solution manager object.
        /// </summary>
        public static IVsSolution VsSolution { get; private set; }

        /// <summary>
        /// Gets the config file object that holds all config functionality.
        /// </summary>
        public static ConfigFile ConfigFile { get; set; }

        protected override void Initialize()
        {
            base.Initialize();

            // Get and set all services
            DTE = (DTE2)GetService(typeof(SDTE));
            RunningDocumentTable = (IVsRunningDocumentTable)GetService(typeof(SVsRunningDocumentTable));
            VsSolution = (IVsSolution)GetService(typeof(SVsSolution));

            var packageLoader = FactoryMethods.GetPackageLoader();
            packageLoader.AfterOpenSolution += PackageLoader_AfterOpenSolution;

            SettingsWindowCommand.Initialize(this);
        }

        private void PackageLoader_AfterOpenSolution()
        {
            ConfigFile = new ConfigFile(DTE.Solution.GetDirectory());
            ConfigFile.Saved += Load;
            _directoryChangeNotifier = new DirectoryChangeNotifier(RunningDocumentTable);

            if (ConfigFile.IsOnDisk)
            {
                Load();
            }
        }

        private void Load()
        {
            ConfigFile.Load();

            if (!_dllsCopied)
            {
                AssemblyExtensions.CopyDllsToGeneratorDirectory(ConfigFile.GeneratorPath, DTE.Solution.GetDirectory());
                _dllsCopied = true;
            }

            // Pre clean up 
            CleanupDirectoryChangeListeners();

            foreach (string uniqueProjectName in ConfigFile.GetProjectNames())
            {
                string[] triggers = ConfigFile.GetTriggers(uniqueProjectName);
                if (triggers.Length == 0)
                    continue;

                Project project = DTE.Solution.FindProject(uniqueProjectName);
                var directoryChangeListener = FactoryMethods.GetRelativeDirectoryChangeListener(project.GetDirectory());
                var generatorRunner = new GeneratorRunner(ConfigFile.GeneratorPath, uniqueProjectName);
                directoryChangeListener.Add(triggers);
                directoryChangeListener.Changed += () => generatorRunner.Run();

                // add to cache (to delete it later)
                _directoryChangeNotifier.Add(directoryChangeListener);
            }
        }

        private void CleanupDirectoryChangeListeners()
        {
            _directoryChangeNotifier.Clear();
        }
    }
}
