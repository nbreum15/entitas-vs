using System;
using System.Windows.Input;
using Entitas_vs.Common;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Entitas_vs.View.Commands
{
    class AddFolderPathCommand : ICommand
    {
        private readonly Action<string> _setFolderValue;
        private readonly string _baseDirectory;
        private readonly bool _isFolderPicker;
        private readonly string _title;

        public AddFolderPathCommand(Action<string> setFolderValue, string baseDirectory, bool isFolderPicker = true, string title = "Select a folder")
        {
            _setFolderValue = setFolderValue;
            _baseDirectory = baseDirectory;
            _isFolderPicker = isFolderPicker;
            _title = title;
        }

        public event EventHandler CanExecuteChanged;
        protected bool IsFolderSet { get; private set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            var commonBrowser = new CommonOpenFileDialog
            {
                Title = _title,
                IsFolderPicker = _isFolderPicker,
                InitialDirectory = _baseDirectory
            };

            if (commonBrowser.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string path = commonBrowser.FileName;
                string relativePath = PathUtil.AbsoluteToRelativePath(_baseDirectory, path);
                _setFolderValue(relativePath);
                IsFolderSet = true;
            }
        }
    }
}
