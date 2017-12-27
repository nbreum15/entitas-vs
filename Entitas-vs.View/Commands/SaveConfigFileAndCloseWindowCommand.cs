using Entitas_vs.View.ViewModels;

namespace Entitas_vs.View.Commands
{
    class SaveConfigFileAndCloseWindowCommand : SaveConfigFileCommand
    {
        public SaveConfigFileAndCloseWindowCommand(SettingsViewModel viewModel, string solutionDirectory) 
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
