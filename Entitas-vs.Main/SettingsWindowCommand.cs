using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;
using EntitasVSGenerator.Extensions;
using EntitasVSGenerator.Logic;
using Microsoft.VisualStudio.Shell;
using IServiceProvider = System.IServiceProvider;
using EntitasVSGenerator.ViewLogic.ViewModels;
using EnvDTE;
using EntitasVSGenerator.ViewLogic.Views;
using EnvDTE80;

namespace EntitasVSGenerator
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class SettingsWindowCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("941919ca-8faf-49f1-90d0-326e0e81ee62");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly EntitasVsPackage _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindowCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private SettingsWindowCommand(EntitasVsPackage package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));

            if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(ShowWindow, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SettingsWindowCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => this._package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(EntitasVsPackage package)
        {
            Instance = new SettingsWindowCommand(package);
        }

        private ConfigFile ConfigFile => EntitasVsPackage.ConfigFile;
        private DTE2 DTE => EntitasVsPackage.DTE;

        /// <summary>
        /// Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void ShowWindow(object sender, EventArgs e)
        {
            if (ConfigFile == null)
            {
                MessageBox.Show("Solution not loaded. Load a solution to see settings.");
                return;
            }
            ConfigFile.Load();
             
            var unusuedProjects = DTE.Solution.GetAllProjects().UniqueNames().Except(ConfigFile.GetProjectNames());

            // Create all tabs
            var settingsViewModel = new SettingsViewModel(ConfigFile); // root tab
            var projectGroupTabViewModel = new ProjectGroupTabViewModel(settingsViewModel);
            var generalTabViewModel = new GeneralTabViewModel(ConfigFile.GeneratorPath,
                DTE.Solution, unusuedProjects, projectGroupTabViewModel, settingsViewModel);
            settingsViewModel.AddChild(generalTabViewModel);
            settingsViewModel.AddChild(projectGroupTabViewModel);

            string[] projectNames = ConfigFile.GetProjectNames();
            foreach (var projectName in projectNames)
            {
                string[] triggers = ConfigFile.GetTriggers(projectName);
                generalTabViewModel.AddProjectTab(projectName, triggers);
            }
            settingsViewModel.AddedProjects = projectGroupTabViewModel.Children;

            var settingsView = new SettingsView { DataContext = settingsViewModel };
            settingsView.Show();
            settingsViewModel.PropertyChanged += (self, args) => { if ((self as SettingsViewModel).WindowClosed) settingsView.Close(); };
        }
    }
}
