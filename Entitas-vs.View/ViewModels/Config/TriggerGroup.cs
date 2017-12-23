using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Entitas_vs.View.ViewModels
{
    class TriggerGroup : BaseViewModel
    {
        private string _generatorName;
        private string _uniqueName;
        private string _entitasCfgPath;
        private string _entitasUserCfgPath;
        private ObservableCollection<string> _triggers;

        [DataMember] public string GeneratorName
        {
            get => _generatorName;
            set => SetValue(ref _generatorName, value);
        }
        [DataMember] public string UniqueName
        {
            get => _uniqueName;
            set => SetValue(ref _uniqueName, value);
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
    }
}