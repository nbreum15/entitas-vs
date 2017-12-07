using System;

namespace EntitasVSGenerator.ViewLogic.Notifications
{
    class VisualStudioNeedsRestartNotification : INotification
    {
        public string Message => "Changes take effect after Visual Studio is restarted.";
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }
}
