using System;
using System.Windows.Input;
using EntitasVSGenerator.ViewLogic.ViewModels;

namespace EntitasVSGenerator.ViewLogic.Commands
{
    class AddProjectCommand : ICommand
    {
        private readonly GeneralTabViewModel _viewModel;

        public AddProjectCommand(GeneralTabViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.AddProjectTab(_viewModel.SelectedProjectName);
            _viewModel.ProjectNames.Remove(_viewModel.SelectedProjectName);
        }

        public event EventHandler CanExecuteChanged;
    }
}
