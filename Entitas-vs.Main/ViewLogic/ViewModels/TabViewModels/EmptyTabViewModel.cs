namespace EntitasVSGenerator.ViewLogic.ViewModels
{
    class EmptyTabViewModel : BaseTabViewModel
    {
        public EmptyTabViewModel(string name, SettingsViewModel settingsViewModel) : base(settingsViewModel) 
        {
            Name = name;
        }

        public override string Name { get; }
    }
}
