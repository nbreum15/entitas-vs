using System;
using System.Windows.Input;
using EntitasVSGenerator.ViewLogic.ViewModels;

namespace EntitasVSGenerator.ViewLogic.Commands
{
    class AddTriggerCommand : ICommand
    {
        private readonly ProjectTabViewModel _viewModel;

        public AddTriggerCommand(ProjectTabViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return string.IsNullOrEmpty((string)parameter);
        }

        public void Execute(object parameter)
        {
            _viewModel.Triggers.Add((string) parameter);
        }
    }
}
