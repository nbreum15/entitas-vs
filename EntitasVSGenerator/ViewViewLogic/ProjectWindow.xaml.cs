namespace EntitasVSGenerator
{
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
            TxtBxProjectName.Text = model.ProjectName;
            model.Changed += ProjectItem_Changed;
        }

        private void ProjectItem_Changed(ProjectItem item, string oldProjectName)
        {
            LstBoxPaths.ItemsSource = Model.Triggers.ToArray();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddPathDialog dialog = new AddPathDialog();
            if (dialog.ShowDialog().Value)
            {
                if (string.IsNullOrEmpty(dialog.Path))
                    return;
                Model.AddTrigger(dialog.Path);
            }
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            if (LstBoxPaths.SelectedItem != null)
            {
                string path = (string)LstBoxPaths.SelectedItem;
                AddPathDialog dialog = new AddPathDialog();
                dialog.txtBoxPath.Text = path;
                if (dialog.ShowDialog().Value)
                {
                    RemoveCurrentSelectedItem();
                    Model.AddTrigger(dialog.Path);
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
    }
}