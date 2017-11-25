using EntitasVSGenerator.Extensions;
using EntitasVSGenerator.Logic;
using System.Windows.Controls;

namespace EntitasVSGenerator.ViewViewLogic
{
    /// <summary>
    /// Interaction logic for ConfigureTab.xaml
    /// </summary>
    public partial class ConfigureTab : UserControl
    {
        public ConfigureTabModel Model { get; set; }

        public ConfigureTab(ConfigureTabModel model)
        {
            InitializeComponent();
            Model = model;
            CreateProjectTabs();
            TxtBxGenPath.Text = Model.GeneratorPath;
            Model.GeneratePathChanged += GeneratePathChanged;
        }

        private void GeneratePathChanged(string path)
        {
            TxtBxGenPath.Text = path;
        }

        private void CreateProjectTabs()
        {
            foreach (ProjectItem item in Model.ProjectItems)
            {
                var projectWindow = new ProjectWindow(item);
                DockPanel.SetDock(projectWindow, Dock.Top);
                ProjectTabContainer.Children.Add(projectWindow);
            }
        }

        private void GenPathButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string selectedFile = DialogUtil.ShowOpenFileDialog(Model.SolutionDirectory, true);
            if (!string.IsNullOrEmpty(selectedFile))
            {
                Model.GeneratorPath = PathUtil.AbsoluteToRelativePath(Model.SolutionDirectory, selectedFile);
            }
        }
    }
}
