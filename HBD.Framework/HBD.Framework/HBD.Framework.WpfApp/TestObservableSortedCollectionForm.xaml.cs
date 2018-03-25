using System.Windows;
using HBD.Framework.Collections;
using HBD.Framework.WpfApp.Base;

namespace HBD.Framework.WpfApp
{
    /// <summary>
    /// Interaction logic for TestObservableSortedCollectionForm.xaml
    /// </summary>
    public partial class TestObservableSortedCollectionForm : Window
    {
        public static readonly DependencyProperty ItemsCollectionProperty = DependencyProperty.Register("ItemsCollection", 
            typeof(ObservableSortedCollection<Item>), typeof(TestObservableSortedCollectionForm));

        public TestObservableSortedCollectionForm()
        {
            InitializeComponent();
            ItemsCollection = new ObservableSortedCollection<Item>(a => a.Index);
        }

        public ObservableSortedCollection<Item> ItemsCollection
        {
            get => GetValue(ItemsCollectionProperty) as ObservableSortedCollection<Item>;
            set => SetValue(ItemsCollectionProperty, value);
        }

        private void BtAdd_OnClick(object sender, RoutedEventArgs e)
        {
            this.ItemsCollection.Add(new Item { Name = TextName.Text, Index = int.Parse(TextIndex.Text) });
            TextIndex.Text = TextName.Text = string.Empty;
        }
    }
}
