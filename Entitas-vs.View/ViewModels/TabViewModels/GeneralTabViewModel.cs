using System.Collections.ObjectModel;
using System.Windows.Input;
using Entitas_vs.View.Commands;

namespace Entitas_vs.View.ViewModels
{
    class GeneralTabViewModel : BaseTabViewModel
    {
        public GeneralTabViewModel(SettingsViewModel settingsViewModel, string solutionDirectory) : base(settingsViewModel)
        {
            Generators = settingsViewModel.ConfigData.Generators;
            AddGeneratorCommand = new AddGeneratorCommand(this, solutionDirectory);
        }

        public override string Name => "General";
        public ObservableCollection<GeneratorData> Generators { get; }
        public ICommand AddGeneratorCommand { get; }
    }
}
