using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Entitas_vs.View.Commands;
using Entitas_vs.View.Views;

namespace Entitas_vs.View.ViewModels
{
    class GeneralTabViewModel : BaseTabViewModel
    {
        public GeneralTabViewModel(SettingsViewModel settingsViewModel, string solutionDirectory) : base(settingsViewModel)
        {
            Generators = settingsViewModel.ConfigData.Generators;
            AddGeneratorCommand = new AddGeneratorCommand(settingsViewModel, solutionDirectory);
            RemoveGeneratorCommand = new RemoveAtIndexCommand(Generators);
            ModifyGeneratorCommand = new ModifyAtIndexCommand(Generators, () => new AddGeneratorView());
        }

        public override string Name => "Generators";
        public ObservableCollection<GeneratorData> Generators { get; }
        public ICommand AddGeneratorCommand { get; }
        public ICommand RemoveGeneratorCommand { get; set; }
        public ICommand ModifyGeneratorCommand { get; set; }
    }
}
