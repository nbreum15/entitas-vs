namespace EntitasVSGenerator.ViewLogic.Notifications
{
    class VisualStudioNeedsRestartNotification : INotification
    {
        public string Message => "Changes take effect after Visual Studio is restarted.";
    }
}
