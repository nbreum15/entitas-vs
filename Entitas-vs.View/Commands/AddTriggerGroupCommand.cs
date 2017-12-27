using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Entitas_vs.View.ViewModels;
using Entitas_vs.View.Views;

namespace Entitas_vs.View.Commands
{
    class AddTriggerGroupCommand : ICommand
    {
        private readonly SettingsViewModel _viewModel;
        private readonly string _solutionDirectory;
        private readonly IEnumerable<string> _uniqueProjectNames;

        public AddTriggerGroupCommand(SettingsViewModel viewModel, string solutionDirectory, IEnumerable<string> uniqueProjectNames)
        {
            _viewModel = viewModel;
            _solutionDirectory = solutionDirectory;
            _uniqueProjectNames = uniqueProjectNames;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            AddTriggerGroupView view = new AddTriggerGroupView();
            var dataContext = new TriggerGroup(_solutionDirectory);
            dataContext.AvailableGenerators = _viewModel.ConfigData.Generators.Select(generator => generator.Name);
            dataContext.AvailableProjects = _uniqueProjectNames;
            view.DataContext = dataContext;

            if (view.ShowDialog() == true)
            {
                _viewModel.ConfigData.TriggerGroups.Add(dataContext);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
