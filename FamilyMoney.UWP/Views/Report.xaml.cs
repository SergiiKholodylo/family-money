using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.Reports;
using FamilyMoneyLib.NetStandard.Reports;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Report : Page
    {
        public Report()
        {
            this.InitializeComponent();
        }

        private void BtTransactionsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Transactions));
        }

        private void BtQuickTransactionButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void BtSettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings.Settings));
        }

        private void BtReportsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Report));
        }

        private void Report1_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage1));
        }

        private void Report2_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CategoryTransactionsReport));
        }
    }
}
