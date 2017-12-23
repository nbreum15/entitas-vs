using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Entitas_vs.Main.Extensions;
using Microsoft.VisualStudio.Shell;
using IServiceProvider = System.IServiceProvider;
using Entitas_vs.View;
using Entitas_vs.View.ViewModels;
using EnvDTE80;
using Entitas_vs.View.Views;

namespace Entitas_vs.Main
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
        
        private DTE2 DTE => EntitasVsPackage.DTE;

        /// <summary>
        /// Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void ShowWindow(object sender, EventArgs e)
        {
            if (!EntitasVsPackage.IsSolutionLoaded)
            {
                MessageBox.Show("Solution not loaded. Load a solution to see settings.");
                return;
            }

            string solutionDirectory = DTE.Solution.GetDirectory();
            ConfigData configData = Config.Load(solutionDirectory);
            var settingsViewModel = new SettingsViewModel(configData, solutionDirectory);

            var settingsView = new SettingsView { DataContext = settingsViewModel, Title = "Entitas VS Settings"};
            settingsView.Show();
            settingsViewModel.PropertyChanged += (self, args) => { if ((self as SettingsViewModel).WindowClosed) settingsView.Close(); };
        }
    }
}
