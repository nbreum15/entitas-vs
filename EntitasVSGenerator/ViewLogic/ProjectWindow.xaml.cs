namespace EntitasVSGenerator
{
    using EntitasVSGenerator.Extensions;
    using EntitasVSGenerator.Logic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for WindowControl.
    /// </summary>
    public partial class ProjectWindow : UserControl
    {
        public ProjectItem Model { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowControl"/> class.
        /// </summary>
        public ProjectWindow(ProjectItem model)
        {
            this.InitializeComponent();
            Model = model;
            LstBoxPaths.ItemsSource = Model.Triggers;
            BtnProjectName.Content = model.ProjectName;
            model.Changed += ProjectItem_Changed;
        }

        private void ProjectItem_Changed(ProjectItem item, string oldProjectName)
        {
            LstBoxPaths.ItemsSource = Model.Triggers.ToArray();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string selectedFile = DialogUtil.ShowOpenFileDialog(Model.Directory, true);
            if (string.IsNullOrEmpty(selectedFile))
                return;
            Model.AddTrigger(PathUtil.AbsoluteToRelativePath(Model.Directory, selectedFile));
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (LstBoxPaths.SelectedItem != null)
            {
                string selectedFile = DialogUtil.ShowOpenFileDialog(Model.Directory, true);
                if (selectedFile != null)
                {
                    RemoveCurrentSelectedItem();
                    Model.AddTrigger(PathUtil.AbsoluteToRelativePath(Model.Directory, selectedFile));
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (LstBoxPaths.SelectedItem != null)
            {
                RemoveCurrentSelectedItem();
            }
        }

        private void RemoveCurrentSelectedItem()
        {
            int index = LstBoxPaths.SelectedIndex;
            Model.RemoveTrigger(LstBoxPaths.SelectedIndex);
        }

        private void BtnProjectName_Click(object sender, RoutedEventArgs e)
        {
            if(ItemContent.Visibility == Visibility.Collapsed)
            {
                ItemContent.Visibility = Visibility.Visible;
            }
            else
            {
                ItemContent.Visibility = Visibility.Collapsed;
            }
        }
    }
}