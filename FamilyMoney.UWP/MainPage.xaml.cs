using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.Bases;
using FamilyMoney.UWP.ViewClasses;
using FamilyMoney.UWP.Views;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;
using ZXing;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FamilyMoney.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static readonly GlobalSettings GlobalSettings = new GlobalSettings();
        readonly MainPageViewModel _viewModel = new MainPageViewModel();

        public MainPage()
        {
            InitializeComponent();
            _viewModel.AddButton(new QuickButton
            {
                Label = "Scan Transaction",
                Symbol = "Camera",
                TransactionId = 0
            });
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

        private async void RunTransaction_Click(object sender, ItemClickEventArgs e)
        {
            await _viewModel.ScanQuickTransaction();

        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await _viewModel.ScanQuickTransaction();
        }
    }
}
