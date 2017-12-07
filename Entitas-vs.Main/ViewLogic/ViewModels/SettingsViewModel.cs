using System.Collections.ObjectModel;
using System.Windows.Input;
using EntitasVSGenerator.Logic;
using EntitasVSGenerator.ViewLogic.Commands;

namespace EntitasVSGenerator.ViewLogic.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        private bool _windowClosed = false;
        private ITabViewModel _currentTabViewModel;
        private ObservableCollection<ITabViewModel> _tabViewModels;

        public SettingsViewModel(ConfigFile configFile)
        {
            SaveConfigFileCommand = new SaveConfigFileCommand(this, configFile);
            SaveConfigFileAndCloseCommand = new SaveConfigFileAndCloseCommand(this, configFile);
        }

        public ICommand SaveConfigFileCommand { get; }
        public ICommand SaveConfigFileAndCloseCommand { get; }

        public ObservableCollection<ITabViewModel> TabViewModels
        {
            get => _tabViewModels;
            set
            {
                _tabViewModels = value;
                CurrentTabViewModel = value[0];
            }
        }

        public ITabViewModel CurrentTabViewModel { get => _currentTabViewModel; set => SetValue(ref _currentTabViewModel, value); }
        public bool WindowClosed { get => _windowClosed; set => SetValue(ref _windowClosed, value); }

        public void CloseWindow()
        {
            WindowClosed = true;
        }
    }
}
