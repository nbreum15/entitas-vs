using Microsoft.WindowsAPICodePack.Dialogs;

namespace Entitas_vs.Main.Extensions
{
    static class DialogUtil
    {
        public static string ShowOpenFileDialog(string initialDirectory, bool allowFolders)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = allowFolders;
            dialog.InitialDirectory = initialDirectory;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                return dialog.FileName;
            else
                return null;
        }
    }
}
