using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Entitas_vs.Main.Extensions;
using Entitas_vs.Main.Logic;
using EnvDTE80;
using Entitas_vs.View;

namespace Entitas_vs.Main
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

        public static bool IsSolutionLoaded { get; private set; }

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
            Config.Saved += Load;
            _directoryChangeNotifier = new DirectoryChangeNotifier(RunningDocumentTable);
            IsSolutionLoaded = true;
        }

        private void Load()
        {
            // Pre clean up 
            _directoryChangeNotifier.ClearListeners();

            var configData = Config.Load(DTE.Solution.GetDirectory());

            foreach (var settingData in configData?.TriggerGroups)
            {
                string generatorPath = configData.GetGeneratorPath(settingData.GeneratorName);
                AssemblyExtensions.CopyDlls(generatorPath, DTE.Solution.GetDirectory());

                if (settingData.Triggers.Count == 0)
                    continue;

                Project project = DTE.Solution.FindProject(settingData.UniqueName);
                var directoryChangeListener = FactoryMethods.GetRelativeDirectoryChangeListener(project.GetDirectory());
                var generatorRunner = new GeneratorRunner(generatorPath, 
                    settingData.UniqueName, settingData.EntitasCfgPath, settingData.EntitasUserCfgPath);
                directoryChangeListener.Add(settingData.Triggers.ToArray());
                directoryChangeListener.Changed += () => generatorRunner.Run();
                _directoryChangeNotifier.AddListener(directoryChangeListener);
            }
        }
    }
}
