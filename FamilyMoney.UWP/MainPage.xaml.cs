using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using FamilyMoney.UWP.Bases;
using FamilyMoney.UWP.Helpers;
using FamilyMoney.UWP.ViewClasses;
using FamilyMoney.UWP.Views;
using FamilyMoney.UWP.Views.Dialogs;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using ZXing;
using Transaction = FamilyMoney.UWP.Views.Transaction;

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
                Label = "➕ Add Quick Transaction",
                TransactionId = 0
            });
            var quickTransactions = GlobalSettings.QuickTransactionStorage.GetAllQuickTransactions();
            foreach (var quickTransaction in quickTransactions)
            {
                _viewModel.AddButton(new QuickButton
                {
                    Label = quickTransaction.Name,
                    TransactionId = quickTransaction.Id,
                    QuickTransaction = quickTransaction
                });
            }
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
            var selected = (QuickButton)e.ClickedItem;
            {
                var quickTransaction = selected.QuickTransaction;
                if (quickTransaction != null)
                {
                    var transaction =
                        QuickTransactionConverter.ToTransaction(new RegularTransactionFactory(), quickTransaction);

                    var isRequireInteraction =
                        QuickTransactionValidator.IsRequireInteractionForTransaction(quickTransaction);

                    if (isRequireInteraction)
                    {
                        var parameters = new TransactionPageParameter( transaction, TransactionAction.CreateTransactionFromTemplate);
                        Frame.Navigate(typeof(Transaction), parameters);
                    }
                    else
                    {
                        try
                        {
                            GlobalSettings.TransactionStorage.CreateTransaction(transaction);
                            var dialog = new ContentDialog
                            {
                                Content = $"Transaction {transaction.Name} was successfully created",
                                IsPrimaryButtonEnabled = true,
                                PrimaryButtonText = "Ok".GetLocalized()
                            };
                            await dialog.ShowAsync();
                        }
                        catch (StorageException exception)
                        {
                            var dialog = new ContentDialog
                            {
                                Content = $"Error during creating Transaction {exception.Message}",
                                IsPrimaryButtonEnabled = true,
                                PrimaryButtonText = "Ok".GetLocalized()
                            };
                            await dialog.ShowAsync();
                        }
                    }
                }
                else
                {
                    var dialog = new EditQuickTransaction(null);
                    var res = await dialog.ShowAsync();
                }

            }

        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await _viewModel.ScanQuickTransaction();
        }

        //private async void GridView_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    var active = (QuickButton)((FrameworkElement) e.OriginalSource).DataContext;
        //    await ShowDialog(active);

        //}

        //private async Task ShowDialog(QuickButton active)
        //{
        //    var dialog = new EditQuickTransaction(active.QuickTransaction);
        //    var res = await dialog.ShowAsync();
        //}
    }
}
