using System;

namespace EntitasVSGenerator.ViewLogic.Notifications
{
    class SolutionNotLoadedNotification : INotification
    {
        public string Message => "Options become available when a solution is loaded.";
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;
    }
}
