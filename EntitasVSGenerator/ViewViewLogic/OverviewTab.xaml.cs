using System.Windows.Controls;

namespace EntitasVSGenerator.ViewViewLogic
{
    /// <summary>
    /// Interaction logic for OverviewTab.xaml
    /// </summary>
    public partial class OverviewTab : UserControl
    {
        public OverviewTabModel Model { get; set; }

        public OverviewTab(OverviewTabModel model)
        {
            InitializeComponent();
            Model = model;
        }
    }
}
