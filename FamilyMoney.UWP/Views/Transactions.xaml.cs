using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.ViewModels;
using FamilyMoneyLib.NetStandard.Bases;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Transactions : Page
    {
        public Transactions()
        {
            this.InitializeComponent();
            ViewModel = new TransactionsViewModel();
        }

        public TransactionsViewModel ViewModel { get; }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddTransaction();
        }
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Transactions));
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Accounts));
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Categories));
        }

        private void CbAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = (IAccount)e.AddedItems.FirstOrDefault();
            ViewModel.SetActiveAccount(selected);
        }
    }
}
