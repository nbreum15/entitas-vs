using System.Windows.Input;

namespace EntitasVSGenerator.ViewLogic.Notifications
{
    interface INotification : ICommand
    {
        string Message { get; }
    }
}
