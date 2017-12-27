using System;
using Entitas_vs.View.ViewModels;
using Entitas_vs.View.Views;

namespace Entitas_vs.View
{
    class Program
    {
        [STAThread]
        static void Main(string[] arguments)
        {
            string solutionDirectory = @"D:\GitHub\entitas-vs\Entitas-vs.View\bin\Debug";
            string[] uniqueProjectNames = {"adwd/awda.csproj", "awidjqqwe.csproj"};
            ConfigData configData = Config.Load(solutionDirectory);
            var settingsViewModel = new SettingsViewModel(configData, solutionDirectory);
            var generalTab = new GeneralTabViewModel(settingsViewModel, solutionDirectory);
            var triggersTab = new TriggersTabViewModel(settingsViewModel, solutionDirectory, uniqueProjectNames);
            settingsViewModel.AddChild(generalTab);
            settingsViewModel.AddChild(triggersTab);
            settingsViewModel.CurrentTabViewModel = generalTab;

            var settingsView = new SettingsView { DataContext = settingsViewModel, Title = "Entitas VS Settings" };
            settingsViewModel.PropertyChanged += (self, args) => { if ((self as SettingsViewModel).WindowClosed) settingsView.Close(); };
            settingsView.ShowDialog();
        }
    }
}
