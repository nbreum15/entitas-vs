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
            string solutionDirectory = "";
            ConfigData configData = Config.Load(solutionDirectory);
            var settingsViewModel = new SettingsViewModel(configData, solutionDirectory);
            var generalTab = new GeneralTabViewModel(settingsViewModel);
            settingsViewModel.AddChild(generalTab);
            settingsViewModel.CurrentTabViewModel = generalTab;

            var settingsView = new SettingsView { DataContext = settingsViewModel, Title = "Entitas VS Settings" };
            settingsView.ShowDialog();
            settingsViewModel.PropertyChanged += (self, args) => { if ((self as SettingsViewModel).WindowClosed) settingsView.Close(); };
        }
    }
}
