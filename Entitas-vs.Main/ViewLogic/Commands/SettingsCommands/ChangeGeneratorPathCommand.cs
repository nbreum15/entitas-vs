using System.Windows.Input;
using System;
using System.Windows;
using EntitasVSGenerator.Extensions;
using EntitasVSGenerator.ViewLogic.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace EntitasVSGenerator.ViewLogic.Commands
{
    class ChangeGeneratorPathCommand : ICommand
    {
        private readonly GeneralTabViewModel _viewModel;
        private readonly string _solutionDirectory;

        public ChangeGeneratorPathCommand(GeneralTabViewModel viewModel, string solutionDirectory)
        {
            _viewModel = viewModel;
            _solutionDirectory = solutionDirectory;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var commonBrowser = new CommonOpenFileDialog
            {
                Title = "Select generator folder",
                IsFolderPicker = true,
                InitialDirectory = _solutionDirectory
            };

            if (commonBrowser.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = commonBrowser.FileName;
                string relative = PathUtil.AbsoluteToRelativePath(_solutionDirectory, path);
                _viewModel.GeneratorPath = relative;
            }
            SettingsWindowCommand.Instance.View.Activate();
        }
    }
}
