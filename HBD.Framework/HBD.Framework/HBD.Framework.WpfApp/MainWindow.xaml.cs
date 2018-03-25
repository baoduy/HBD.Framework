using System.Windows;

namespace HBD.Framework.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnTestObservableSortedCollection_OnClick(object sender, RoutedEventArgs e)
        {
            new TestObservableSortedCollectionForm().ShowDialog();
        }
    }
}
