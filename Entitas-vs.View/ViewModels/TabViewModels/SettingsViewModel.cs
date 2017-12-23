using System.Windows.Input;
using Entitas_vs.View.Commands;

namespace Entitas_vs.View.ViewModels
{
    class SettingsViewModel : BaseTabViewModel
    {
        private bool _windowClosed = false;
        private ITabViewModel _currentTabViewModel;
        private string _notificationMessage;

        public SettingsViewModel(ConfigData configData, string solutionDirectory) : base(null)
        {
            ConfigData = configData;
            SaveConfigFileCommand = new SaveConfigFileCommand(this, solutionDirectory);
            SaveConfigFileAndCloseCommand = new SaveConfigFileAndCloseCommand(this, solutionDirectory);
        }

        public ConfigData ConfigData { get; }

        public override string Name => "Settings";

        public ICommand SaveConfigFileCommand { get; }
        public ICommand SaveConfigFileAndCloseCommand { get; }
        public ITabViewModel CurrentTabViewModel { get => _currentTabViewModel; set => SetValue(ref _currentTabViewModel, value); }
        public bool WindowClosed { get => _windowClosed; set => SetValue(ref _windowClosed, value); }
        public string NotificationMessage { get => _notificationMessage; set => SetValue(ref _notificationMessage, value); }

        public void CloseWindow()
        {
            WindowClosed = true;
        }
    }
}
