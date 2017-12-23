using System.Collections.ObjectModel;

namespace Entitas_vs.View.ViewModels
{
    abstract class BaseTabViewModel : BaseViewModel, ITabViewModel
    {
        public SettingsViewModel SettingsViewModel { get; }
        private bool _isSelected;

        protected BaseTabViewModel(SettingsViewModel settingsViewModel)
        {
            SettingsViewModel = settingsViewModel;
        }

        public abstract string Name { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetValue(ref _isSelected, value);
                SettingsViewModel.CurrentTabViewModel = this;
            }
        }

        public virtual ObservableCollection<ITabViewModel> Children { get; } = new ObservableCollection<ITabViewModel>();

        public void AddChild(ITabViewModel childToAdd)
        {
            Children.Add(childToAdd);
        }
    }
}
