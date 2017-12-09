using System.Collections.ObjectModel;
using System.Windows.Input;
using EntitasVSGenerator.Logic;
using EntitasVSGenerator.ViewLogic.Commands;

namespace EntitasVSGenerator.ViewLogic.ViewModels
{
    class SettingsViewModel : EmptyTabViewModel
    {
        private bool _windowClosed = false;
        private ITabViewModel _currentTabViewModel;

        public SettingsViewModel(ConfigFile configFile) : base("Settings", null)
        {
            SaveConfigFileCommand = new SaveConfigFileCommand(this, configFile);
            SaveConfigFileAndCloseCommand = new SaveConfigFileAndCloseCommand(this, configFile);
        }

        public ICommand SaveConfigFileCommand { get; }
        public ICommand SaveConfigFileAndCloseCommand { get; }
        public ITabViewModel CurrentTabViewModel { get => _currentTabViewModel; set => SetValue(ref _currentTabViewModel, value); }
        public bool WindowClosed { get => _windowClosed; set => SetValue(ref _windowClosed, value); }
        public ObservableCollection<ITabViewModel> AddedProjects { get; set; }

        public void CloseWindow()
        {
            WindowClosed = true;
        }
    }
}
