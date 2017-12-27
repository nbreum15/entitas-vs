using System;
using System.Windows.Input;
using Entitas_vs.View.ViewModels;
using Entitas_vs.View.Views;

namespace Entitas_vs.View.Commands
{
    class AddGeneratorCommand : ICommand
    {
        private readonly SettingsViewModel _viewModel;
        private readonly string _solutionDirectory;

        public AddGeneratorCommand(SettingsViewModel viewModel, string solutionDirectory)
        {
            _viewModel = viewModel;
            _solutionDirectory = solutionDirectory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AddGeneratorView view = new AddGeneratorView();
            var dataContext = new GeneratorData(_solutionDirectory);
            view.DataContext = dataContext;
            
            if (view.ShowDialog() == true)
            {
                _viewModel.ConfigData.Generators.Add(dataContext);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
