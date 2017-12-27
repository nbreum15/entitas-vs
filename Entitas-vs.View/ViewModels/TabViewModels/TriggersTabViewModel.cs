using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Entitas_vs.View.Commands;
using Entitas_vs.View.Views;

namespace Entitas_vs.View.ViewModels
{
    class TriggersTabViewModel : BaseTabViewModel
    {
        public TriggersTabViewModel(SettingsViewModel settingsViewModel, string solutionDirectory, IEnumerable<string> uniqueProjectNames) : base(settingsViewModel)
        {
            TriggerGroups = settingsViewModel.ConfigData.TriggerGroups;
            AddTriggerGroupCommand = new AddTriggerGroupCommand(settingsViewModel, solutionDirectory, uniqueProjectNames);
            RemoveTriggerGroupCommand = new RemoveAtIndexCommand(TriggerGroups);
            ModifyTriggerGroupCommand = new ModifyAtIndexCommand(TriggerGroups, () => new AddTriggerGroupView());
        }

        public override string Name => "Triggers";
        public ObservableCollection<TriggerGroup> TriggerGroups { get; }
        public ICommand AddTriggerGroupCommand { get; set; }
        public ICommand RemoveTriggerGroupCommand { get; set; }
        public ICommand ModifyTriggerGroupCommand { get; set; }
    }
}
