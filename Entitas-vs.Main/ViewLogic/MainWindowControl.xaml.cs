namespace EntitasVSGenerator
{
    using EntitasVSGenerator.ViewViewLogic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for WindowControl.
    /// </summary>
    public partial class MainWindowControl : UserControl
    {
        private const int OverviewTab = 0;
        private const int ConfigureTab = 1;

        private MainWindowModel _model;
        public MainWindowModel Model
        {
            get => _model;
            set
            {
                _model = value;
                ShowTabAtIndex(TabSelect.SelectedIndex);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowControl"/> class.
        /// </summary>
        public MainWindowControl()
        {
            this.InitializeComponent();
            ShowTabAtIndex(OverviewTab);
        }

        private void TabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowTabAtIndex((sender as ComboBox).SelectedIndex);
        }

        private void ShowTabAtIndex(int selectedIndex)
        {
            if (Model == null)
                return;
            switch (selectedIndex)
            {
                case OverviewTab: // overview
                    ShowTab(new OverviewTab(Model.OverviewTabModel));
                    break;
                case ConfigureTab: // configure
                    ShowTab(new ConfigureTab(Model.ConfigureTabModel));
                    break;
                default: // default to overview tab if something goes wrong
                    ShowTab(new OverviewTab(Model.OverviewTabModel));
                    break;
            }
        }

        private void ShowTab(UIElement toShow)
        {
            var viewContainer = ViewContainer;
            viewContainer?.Children.Clear();
            viewContainer?.Children.Add(toShow);
        }
    }
}