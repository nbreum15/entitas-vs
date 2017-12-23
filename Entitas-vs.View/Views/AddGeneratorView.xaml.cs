using System.Windows;

namespace Entitas_vs.View.Views
{
    /// <summary>
    /// Interaction logic for AddGeneratorView.xaml
    /// </summary>
    public partial class AddGeneratorView : Window
    {
        public AddGeneratorView()
        {
            InitializeComponent();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
