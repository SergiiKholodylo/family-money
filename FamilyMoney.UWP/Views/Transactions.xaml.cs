using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using FamilyMoney.UWP.Helpers;
using FamilyMoney.UWP.ViewModels;
using FamilyMoney.UWP.Views.Dialogs;
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

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var addTransaction
                = new AddTransaction(ViewModel.ActiveAccount);

            var result = await addTransaction.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.RefreshTransactionByAccount();
            }
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

        private async void DeleteItem_ItemInvoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            var activeTransaction = (ITransaction)args.SwipeControl.DataContext;
            var deleteConfirmation = new ContentDialog
            {
                Title = "DeleteTransaction".GetLocalized(),
                PrimaryButtonText = "DeleteTransaction".GetLocalized(),
                SecondaryButtonText = "Cancel".GetLocalized(),
                DefaultButton = ContentDialogButton.Primary,
                Content = $"Do you want delete transaction \n '{activeTransaction.Name}({activeTransaction.Total})'?"
            };

            var result = await deleteConfirmation.ShowAsync();

            if (result == ContentDialogResult.Primary)
                ViewModel.DeleteTransaction(activeTransaction);
        }

        private async void ListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var activeTransaction = (ITransaction)(((ListView)sender).SelectedValue);
            var editTransaction = new EditTransaction(activeTransaction);
            var result = await editTransaction.ShowAsync();
            if (result == ContentDialogResult.Primary)
                ViewModel.RefreshTransactionByAccount();
        }
    }
}
