using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows.Input;
using Entitas_vs.View.Commands;

namespace Entitas_vs.View.ViewModels
{
    [DataContract]
    class TriggerGroup : BaseViewModel
    {
        private string _generatorName;
        private string _uniqueProjectName;
        private string _entitasCfgPath;
        private string _entitasUserCfgPath;
        private ObservableCollection<string> _triggers;

        public TriggerGroup(string solutionDirectory)
        {
            AddTriggerCommand = new AddFolderPathCommand(path => Triggers.Add(path), solutionDirectory);
            RemoveTriggerCommand = new RemoveAtIndexCommand(Triggers);
            AddEntitasCfgCommand = new AddFolderPathCommand(path => EntitasCfgPath = path, solutionDirectory, false);
            AddEntitasUserCfgCommand = new AddFolderPathCommand(path => EntitasUserCfgPath = path, solutionDirectory, false);
            Triggers = new ObservableCollection<string>();
        }

        public IEnumerable<string> AvailableGenerators { get; set; }
        public IEnumerable<string> AvailableProjects { get; set; }
        public ICommand AddTriggerCommand { get; set; }
        public ICommand RemoveTriggerCommand { get; set; }
        public ICommand AddEntitasCfgCommand { get; set; }
        public ICommand AddEntitasUserCfgCommand { get; set; }

        [DataMember] public string GeneratorName
        {
            get => _generatorName;
            set => SetValue(ref _generatorName, value);
        }
        [DataMember] public string UniqueProjectName
        {
            get => _uniqueProjectName;
            set => SetValue(ref _uniqueProjectName, value);
        }
        [DataMember] public string EntitasCfgPath
        {
            get => _entitasCfgPath;
            set => SetValue(ref _entitasCfgPath, value);
        }
        [DataMember] public string EntitasUserCfgPath
        {
            get => _entitasUserCfgPath;
            set => SetValue(ref _entitasUserCfgPath, value);
        }
        [DataMember] public ObservableCollection<string> Triggers 
        {
            get => _triggers;
            set => SetValue(ref _triggers, value);
        }

        public string DisplayName => $"{Triggers.Count} {(Triggers.Count == 1 ? "Trigger" : "Triggers")} (Generator: {GeneratorName}, Project: {UniqueProjectName})";
    }
}