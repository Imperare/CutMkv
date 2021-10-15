using CutMkv.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace CutMkv.View
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel.Instance;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    if (file.EndsWith(".mkv"))
                        MainViewModel.Instance.EmplacementVideo = file;
                    else if (file.EndsWith(".txt"))
                        MainViewModel.Instance.ChargerFichierTxt(file);
                }
            }
        }

        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
    }
}
