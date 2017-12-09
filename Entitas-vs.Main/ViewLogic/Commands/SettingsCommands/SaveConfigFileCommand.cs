using System;
using System.Linq;
using System.Windows.Input;
using EntitasVSGenerator.Logic;
using EntitasVSGenerator.ViewLogic.ViewModels;
using MoreLinq;

namespace EntitasVSGenerator.ViewLogic.Commands
{
    class SaveConfigFileCommand : ICommand
    {
        protected SettingsViewModel ViewModel { get; }
        protected ConfigFile ConfigFile { get; }

        public SaveConfigFileCommand(SettingsViewModel viewModel, ConfigFile configFile)
        {
            ViewModel = viewModel;
            ConfigFile = configFile;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            var generalTab = ViewModel.Children.OfType<GeneralTabViewModel>().First();
            ConfigFile.GeneratorPath = generalTab.GeneratorPath;

            var projectTabs = ViewModel.Children.OfType<ProjectTabViewModel>();
            projectTabs.ForEach(model => ConfigFile.Refresh(model.Name, model.Triggers.ToArray()));
            ConfigFile.Save();
        }

        public virtual event EventHandler CanExecuteChanged;
    }
}
