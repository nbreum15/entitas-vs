using EntitasVSGenerator.Logic;
using EntitasVSGenerator.ViewLogic.ViewModels;

namespace EntitasVSGenerator.ViewLogic.Commands
{
    class SaveConfigFileAndCloseCommand : SaveConfigFileCommand
    {
        public SaveConfigFileAndCloseCommand(SettingsViewModel viewModel, ConfigFile configFile) 
            : base(viewModel, configFile)
        {
        }

        public override void Execute(object parameter)
        {
            ViewModel.CloseWindow();
            base.Execute(parameter);
        }
    }
}
