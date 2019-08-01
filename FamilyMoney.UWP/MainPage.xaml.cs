using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FamilyMoney.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static readonly GlobalSettings GlobalSettings = new GlobalSettings();
        public MainPage()
        {
            InitializeComponent();
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Accounts));
        }

        private void AppBarButton_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Categories));
        }

        private void AppBarButton_Click_2(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Transactions));
        }
    }
}
