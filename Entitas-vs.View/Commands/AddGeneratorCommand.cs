using System;
using System.Windows.Input;
using Entitas_vs.View.ViewModels;
using Entitas_vs.View.Views;

namespace Entitas_vs.View.Commands
{
    class AddGeneratorCommand : ICommand
    {
        private readonly GeneralTabViewModel _viewModel;
        private readonly string _solutionDirectory;

        public AddGeneratorCommand(GeneralTabViewModel viewModel, string solutionDirectory)
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
            var data = new GeneratorData(_solutionDirectory);
            view.DataContext = data;
            
            if (view.ShowDialog() == true)
            {
                _viewModel.SettingsViewModel.ConfigData.Generators.Add(data);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
