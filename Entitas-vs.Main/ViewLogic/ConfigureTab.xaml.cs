using EntitasVSGenerator.Extensions;
using EntitasVSGenerator.Logic;
using System.Windows.Controls;
using System.Windows;

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
            if (!Model.IsGeneratorLoaded)
            {
                TxtBlckGeneratorNotice.Text = "Generator not started because generator path is not set.";
                TxtBlckGeneratorNotice.Visibility = Visibility.Visible;
            }
        }

        private void GeneratePathChanged(string path)
        {
            TxtBxGenPath.Text = path;
            if (!Model.IsGeneratorLoaded)
            {
                TxtBlckGeneratorNotice.Text = "Do you want to start the generator? (generator will start automatically on next launch)";
                BtnGeneratorLoadYes.Visibility = Visibility.Visible;
            }
            else
            {
                TxtBlckGeneratorNotice.Text = "Change will take effect after Visual Studio restart.";
                BtnGeneratorLoadYes.Visibility = Visibility.Visible;
            }
        }

        private void CreateProjectTabs()
        {
            foreach (ProjectViewModel item in Model.ProjectViewModels)
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

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Model.LoadGenerator();
            BtnGeneratorLoadYes.Visibility = Visibility.Collapsed;
            TxtBlckGeneratorNotice.Visibility = Visibility.Collapsed;
        }
    }
}
