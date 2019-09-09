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
    public sealed partial class BarCodes : Page
    {
        private bool _isDialogActive = false;
        public BarCodeViewModel ViewModel { get; }

        public BarCodes()
        {
            this.InitializeComponent();
            ViewModel = new BarCodeViewModel(MainPage.GlobalSettings.Storages.BarCodeStorage);
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


        private void ListView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var barCode = ((FrameworkElement) e.OriginalSource).DataContext as IBarCode;
            ViewModel.InverseIsWeight(barCode);
        }

        private async void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var activeBarCode = (IBarCode)((FrameworkElement)e.OriginalSource).DataContext;
            if (activeBarCode == null) return;
            await DeleteBarCode(activeBarCode);
        }

        private async void ListView_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var activeBarCode = (IBarCode)((ListView)sender).SelectedValue;
            if (activeBarCode == null) return;
            await DeleteBarCode(activeBarCode);
        }

        private async System.Threading.Tasks.Task DeleteBarCode(IBarCode barCode)
        {
            if (_isDialogActive) return;
            var dialog = new ContentDialog
            {
                Title = "Delete Transaction",
                Content = $"Delete {barCode.Code}?",
                IsPrimaryButtonEnabled = true,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "Delete",
                SecondaryButtonText = "Cancel"
            };
            _isDialogActive = true;
            var res = await dialog.ShowAsync();
            _isDialogActive = false;
            if (res == ContentDialogResult.Secondary) return;
            ViewModel.DeleteBarCode(barCode);
        }
    }
}
