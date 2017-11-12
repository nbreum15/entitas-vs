namespace EntitasVSGenerator
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for WindowControl.
    /// </summary>
    public partial class WindowControl : UserControl
    {
        PathContainer _pathContainer; 
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControl"/> class.
        /// </summary>
        public WindowControl()
        {
            this.InitializeComponent();
            _pathContainer = MainWindowCommand.Instance.PathContainer;
            LstBoxPaths.ItemsSource = _pathContainer.AllPaths;
            _pathContainer.Changed += FileTrigger_Changed;
        }

        private void FileTrigger_Changed(string[] obj)
        {
            LstBoxPaths.ItemsSource = obj;
            _pathContainer.SavePaths(obj);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddPathDialog dialog = new AddPathDialog();
            if (dialog.ShowDialog().Value)
            {
                if (string.IsNullOrEmpty(dialog.Path))
                    return;
                _pathContainer.Add(dialog.Path);
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
                    _pathContainer.Remove(path);
                    _pathContainer.Add(dialog.Path);
                }
            }            
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(LstBoxPaths.SelectedItem != null)
            {
                string path = (string)LstBoxPaths.SelectedItem;
                _pathContainer.Remove(path);
            }
        }
    }
}