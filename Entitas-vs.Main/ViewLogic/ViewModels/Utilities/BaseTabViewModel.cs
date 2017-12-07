namespace EntitasVSGenerator.ViewLogic.ViewModels
{
    abstract class BaseTabViewModel : BaseViewModel, ITabViewModel
    {
        public SettingsViewModel SettingsViewModel { get; set; }
        private bool _isSelected;

        public abstract string Name { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SettingsViewModel.CurrentTabViewModel = this;
                SetValue(ref _isSelected, value);
            }
        }
    }
}
