namespace EntitasVSGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for WindowControl.
    /// </summary>
    public partial class ConfigureWindow : UserControl
    {
        private ConfigureWindowModel _model;
        public ConfigureWindowModel Model
        {
            get => _model; set
            {
                LstBoxPaths.ItemsSource = value.Paths;
                _model = value;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowControl"/> class.
        /// </summary>
        public ConfigureWindow()
        {
            this.InitializeComponent();
            LstBoxPaths.ItemsSource = Model?.Paths;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddPathDialog dialog = new AddPathDialog();
            if (dialog.ShowDialog().Value)
            {
                if (string.IsNullOrEmpty(dialog.Path))
                    return;
                Model?.Add(dialog.Path);
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
                    Model?.Remove(path);
                    Model?.Add(dialog.Path);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Model.DebugAction();
            if (LstBoxPaths.SelectedItem != null)
            {
                string path = (string)LstBoxPaths.SelectedItem;
                Model?.Remove(path);
            }
        }
    }

    public class ConfigureWindowModel
    {
        public ConfigureWindowModel(IEnumerable<string> paths)
        {
            Paths = new ObservableCollection<string>(paths);
        }

        public ObservableCollection<string> Paths { get; set; } = new ObservableCollection<string>();
        public Action DebugAction { get; set; }

        public void Add(string path)
        {
            Paths.Add(path);
        }

        public void Remove(string path)
        {
            Paths.Remove(path);
        }
    }