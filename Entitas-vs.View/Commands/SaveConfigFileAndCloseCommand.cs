using Entitas_vs.View.ViewModels;

namespace Entitas_vs.View.Commands
{
    class SaveConfigFileAndCloseCommand : SaveConfigFileCommand
    {
        public SaveConfigFileAndCloseCommand(SettingsViewModel viewModel, string solutionDirectory) 
            : base(viewModel, solutionDirectory)
        {
        }

        public override void Execute(object parameter)
        {
            ViewModel.CloseWindow();
            base.Execute(parameter);
        }
    }
}
