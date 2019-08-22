using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using FamilyMoney.UWP.Bases;
using FamilyMoney.UWP.Views.Dialogs;
using FamilyMoney.ViewModels.NetStandard.ViewModels;
using FamilyMoneyLib.NetStandard.Bases;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FamilyMoney.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Transaction : Page
    {
        public ITransactionViewModel ViewModel;
        private ITransaction transaction;

        public Transaction()
        {
            this.InitializeComponent();
        }

        public Transaction(ITransaction transaction)
        {
            this.transaction = transaction;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var storages = MainPage.GlobalSettings.Storages;
            var parameter = e.Parameter as TransactionPageParameter;
            if (e.NavigationMode == NavigationMode.New)
            {
                switch (parameter?.Action)
                {
                    case TransactionAction.CreateNewTransaction:

                    case TransactionAction.CreateTransactionForAccount:
                        ViewModel = new TransactionCreateViewModel(storages, parameter?.ActiveAccount);
                        break;
                    case TransactionAction.EditTransaction:
                        ViewModel = new TransactionEditViewModel(storages, parameter?.ActiveTransaction);
                        break;
                    case TransactionAction.CreateTransactionFromTemplate:
                        ViewModel = new TransactionCreateViewModel(storages, parameter?.ActiveTransaction);
                        break;
                    default:
                        ViewModel = new TransactionCreateViewModel(storages);
                        break;
                }

                MainPage.GlobalSettings.FormsView.Transaction = ViewModel;
            }
            else //Back event
            {
                ViewModel = MainPage.GlobalSettings.FormsView.Transaction ?? new TransactionCreateViewModel(storages);
            }
        }



        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var selected = args.SelectedItem as ITransaction;
            ViewModel.FillFromTemplate(selected);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput) return;
            var searchString = sender.Text;
            sender.ItemsSource = ViewModel.GetSuggestions(searchString);
        }

        private void CommandBar_SaveButton(object sender, RoutedEventArgs e)
        {
            SaveTransaction();
        }

        private void CommandBar_CancelButton(object sender, RoutedEventArgs e)
        {
            ExitWithoutSaving();
        }

        private async void CommandBar_AddChildTransaction(object sender, RoutedEventArgs e)
        {
            await AddChildTransaction();
        }

        private async void ChildrenTransaction_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            await EditChildTransaction(sender);
        }

        private async Task AddChildTransaction()
        {
            var editTransaction = new EditChildTransaction(ViewModel.Transaction, ViewModel.Account);
            var result = await editTransaction.ShowAsync();
            ViewModel.UpdateChildrenTransactionList();
        }
        private async Task EditChildTransaction(object sender)
        {
            var activeTransaction = (ITransaction)((Windows.UI.Xaml.Controls.Primitives.Selector)sender).SelectedValue;
            var editTransaction = new EditChildTransaction(ViewModel.Transaction, ViewModel.Account, activeTransaction);
            var result = await editTransaction.ShowAsync();
        }

        private async void SaveTransaction()
        {

            try
            {
                ViewModel.SaveTransaction();
                Frame.Navigate(typeof(Transactions));
            }
            catch (ViewModelException e)
            {
                var dialog = new ContentDialog
                {
                    Content = e.Message,
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = "Ok"
                };
                await dialog.ShowAsync();
            }
        }
        private void ExitWithoutSaving()
        {
            Frame.Navigate(typeof(Transactions));
        }

        private async void CommandBar_ScanBarCode(object sender, RoutedEventArgs e)
        {
            var scannedCode = await ViewModel.ScanBarCode();

            ViewModel.ProcessScannedBarCode(scannedCode);
        }

        private async void CommandBar_ScanBarChildTransactionCode(object sender, RoutedEventArgs e)
        {
            //var childTransaction = await ViewModel.ScanChildTransaction();
            //await childTransaction.ShowAsync();
        }
    }
}
