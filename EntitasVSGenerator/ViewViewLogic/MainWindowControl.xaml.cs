namespace EntitasVSGenerator
{
    using EntitasVSGenerator.ViewViewLogic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for WindowControl.
    /// </summary>
    public partial class WindowControl : UserControl
    {
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
        /// Initializes a new instance of the <see cref="WindowControl"/> class.
        /// </summary>
        public WindowControl()
        {
            this.InitializeComponent();
            ShowTab(new OverviewTab(Model?.OverviewModel));
        }

        private void TabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowTabAtIndex((sender as ComboBox).SelectedIndex);
        }

        private void ShowTabAtIndex(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0: // overview
                    ShowTab(new OverviewTab(Model?.OverviewModel));
                    break;
                case 1: // configure
                    ShowTab(new ConfigureTab(Model?.ConfigureModel));
                    break;
                default:
                    ShowTab(new OverviewTab(Model?.OverviewModel));
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

    public class MainWindowModel
    {
        public ConfigureTabModel ConfigureModel { get; set; }
        public OverviewTabModel OverviewModel { get; set; }

        public MainWindowModel()
        {
        }
    }

    public class OverviewTabModel
    {

    }
}