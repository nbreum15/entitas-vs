using System.Collections.ObjectModel;

namespace Entitas_vs.View.ViewModels
{
    class GeneralTabViewModel : BaseTabViewModel
    {
        public GeneralTabViewModel(SettingsViewModel settingsViewModel) : base(settingsViewModel)
        {
            Generators = settingsViewModel.ConfigData.Generators;

        }

        public override string Name => "General";
        public ObservableCollection<GeneratorData> Generators { get; }
    }
}
