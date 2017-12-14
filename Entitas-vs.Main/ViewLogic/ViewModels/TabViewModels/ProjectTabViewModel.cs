using System.Collections.ObjectModel;
using System.Windows.Input;
using EntitasVSGenerator.ViewLogic.Commands;

namespace EntitasVSGenerator.ViewLogic.ViewModels
{
    class ProjectTabViewModel : BaseTabViewModel
    {
        public ICommand AddTriggerCommand { get; }
        public ICommand DeleteTriggerCommand { get; }

        public ObservableCollection<string> Triggers { get; }
        public override string Name { get; }

        public ProjectTabViewModel(string name, 
            string projectDirectory, SettingsViewModel settingsViewModel, 
            string[] triggers = null) : base(settingsViewModel)
        {
            Name = name;
            Triggers = new ObservableCollection<string>(triggers ?? new string[0]);
            AddTriggerCommand = new AddTriggerCommand(this, projectDirectory);
            DeleteTriggerCommand = new DeleteTriggerCommand(this);
        }
    }
}
