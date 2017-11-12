using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace EntitasVSGenerator
{
    /// <summary>
    /// Interaction logic for AddPathDialog.xaml
    /// </summary>
    public partial class AddPathDialog : System.Windows.Window
    {
        public string Path { get; set; }

        public AddPathDialog()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Path = txtBoxPath.Text;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtBoxPath.Text = openFileDialog.FileName;
        }
    }
}
