using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EntitasVSGenerator.Extensions;
using EntitasVSGenerator.ViewLogic.Commands;
using EnvDTE;

namespace EntitasVSGenerator.ViewLogic.ViewModels
{
    class GeneralTabViewModel : BaseTabViewModel
    {
        private readonly Solution _solution;
        private string _generatorPath;
        private string _selectedProjectName;

        public GeneralTabViewModel(string generatorPath, 
            Solution solution, 
            IEnumerable<string> projectNames,
            ITabViewModel projectGroupTab, 
            SettingsViewModel settingsViewModel) : base(settingsViewModel)
        {
            _solution = solution;
            GeneratorPath = generatorPath;
            ProjectGroupTab = projectGroupTab;
            ProjectNames = new ObservableCollection<string>(projectNames);
            ChangeGeneratorPathCommand = new ChangeGeneratorPathCommand(this, solution.GetDirectory());
            AddProjectCommand = new AddProjectCommand(this);
        }

        public ICommand ChangeGeneratorPathCommand { get; }
        public ICommand AddProjectCommand { get; }
        public override string Name => "General";

        public string SelectedProjectName { get => _selectedProjectName; set => SetValue(ref _selectedProjectName, value); }
        public string GeneratorPath { get => _generatorPath; set => SetValue(ref _generatorPath, value); }
        private ITabViewModel ProjectGroupTab { get; }

        public ObservableCollection<string> ProjectNames { get; }

        public void AddProjectTab(string name, string[] triggers = null)
        {
            string projectDir = _solution.FindProject(name).GetDirectory();
            var projectTabViewModel = new ProjectTabViewModel(name, projectDir, SettingsViewModel, triggers);
            ProjectGroupTab.AddChild(projectTabViewModel);
        }
    }
}
