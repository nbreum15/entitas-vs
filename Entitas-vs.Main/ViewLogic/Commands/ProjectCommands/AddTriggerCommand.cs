using System;
using System.Windows.Input;
using EntitasVSGenerator.ViewLogic.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace EntitasVSGenerator.ViewLogic.Commands
{
    class AddTriggerCommand : ICommand
    {
        private readonly ProjectTabViewModel _viewModel;
        private readonly string _projectDirectory;

        public AddTriggerCommand(ProjectTabViewModel viewModel, string projectDirectory)
        {
            _viewModel = viewModel;
            _projectDirectory = projectDirectory;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return string.IsNullOrEmpty((string)parameter);
        }

        public void Execute(object parameter)
        {
            var commonBrowser = new CommonOpenFileDialog
            {
                Title = "Select a folder",
                IsFolderPicker = true,
                InitialDirectory = _projectDirectory
            };

            if (commonBrowser.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = commonBrowser.FileName;
                string relative = path.Replace(_projectDirectory, "");
                _viewModel.Triggers.Add(relative);
            }
        }
    }
}
