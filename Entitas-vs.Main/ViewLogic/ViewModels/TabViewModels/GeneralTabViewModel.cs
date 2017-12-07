using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EntitasVSGenerator.ViewLogic.Commands;

namespace EntitasVSGenerator.ViewLogic.ViewModels
{
    class GeneralTabViewModel : BaseTabViewModel
    {
        private string _generatorPath;
        private string _selectedProjectName;

        public GeneralTabViewModel(string generatorPath, 
            string solutionDirectory, 
            IEnumerable<string> projectNames)
        {
            GeneratorPath = generatorPath;
            ProjectNames = new ObservableCollection<string>(projectNames);
            ChangeGeneratorPathCommand = new ChangeGeneratorPathCommand(this, solutionDirectory);
            AddProjectCommand = new AddProjectCommand(this);
        }

        public ICommand ChangeGeneratorPathCommand { get; }
        public ICommand AddProjectCommand { get; }
        public override string Name => "General";

        public string SelectedProjectName { get => _selectedProjectName; set => SetValue(ref _selectedProjectName, value); }
        public string GeneratorPath { get => _generatorPath; set => SetValue(ref _generatorPath, value); }

        public ObservableCollection<string> ProjectNames { get; }

        public void AddProjectTab(string name)
        {
            SettingsViewModel.TabViewModels.Add(new ProjectTabViewModel(name){SettingsViewModel = SettingsViewModel});
        }
    }
}
