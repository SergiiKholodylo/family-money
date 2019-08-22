using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using FamilyMoney.ViewModels.NetStandard.ViewModels;
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
            ViewModel = new TransactionsViewModel(MainPage.GlobalSettings.Storages);
        }

        public TransactionsViewModel ViewModel { get; }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var parameter = new TransactionPageParameter(ViewModel.ActiveAccount);
            Frame.Navigate(typeof(Transaction), parameter);
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

        private async void DeleteItem_ItemInvoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            var activeTransaction = (ITransaction)args.SwipeControl.DataContext;
            var deleteConfirmation = new ContentDialog
            {
                Title = "DeleteTransaction",
                PrimaryButtonText = "DeleteTransaction",
                SecondaryButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = $"Do you want delete transaction \n '{activeTransaction.Name}({activeTransaction.Total})'?"
            };

            var result = await deleteConfirmation.ShowAsync();

            if (result == ContentDialogResult.Primary)
                ViewModel.DeleteTransaction(activeTransaction);
        }

        private void ListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var activeTransaction = (ITransaction)(((ListView)sender).SelectedValue);
            var parameter = new TransactionPageParameter(activeTransaction);
            Frame.Navigate(typeof(Transaction), parameter);
        }

        private async void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var activeTransaction = (ITransaction)((FrameworkElement)e.OriginalSource).DataContext;
            if(activeTransaction==null) return;

            var dialog = new ContentDialog
            {
                Title = "ListView_RightTapped",
                Content = $"Delete {activeTransaction.Name}?",
                IsPrimaryButtonEnabled = true,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel"
            };
            var res = await dialog.ShowAsync();
            if(res == ContentDialogResult.Secondary) return;
            ViewModel.DeleteTransaction(activeTransaction);
        }


        private async void ListView_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var activeTransaction = (ITransaction)(((ListView)sender).SelectedValue);
            if (activeTransaction == null) return;


            var dialog = new ContentDialog
            {
                Title = "ListView_Holding",
                Content = $"Delete {activeTransaction.Name}?",
                IsPrimaryButtonEnabled = true,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel"
            };
            var res = await dialog.ShowAsync();
            if(res == ContentDialogResult.Secondary) return;
            ViewModel.DeleteTransaction(activeTransaction);
        }
    }
}
