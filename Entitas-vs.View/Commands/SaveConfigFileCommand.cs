using System;
using System.Windows.Input;
using Entitas_vs.View.ViewModels;

namespace Entitas_vs.View.Commands
{
    class SaveConfigFileCommand : ICommand
    {
        private readonly string _solutionDirectory;
        protected SettingsViewModel ViewModel { get; }

        public SaveConfigFileCommand(SettingsViewModel viewModel, string solutionDirectory)
        {
            _solutionDirectory = solutionDirectory;
            ViewModel = viewModel;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            Config.Save(ViewModel.ConfigData, _solutionDirectory);
        }

        public virtual event EventHandler CanExecuteChanged;
    }
}
