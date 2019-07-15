using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Categories : Page
    {
        public CategoryViewModel ViewModel = new CategoryViewModel();

        public Categories()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddCategory();
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
