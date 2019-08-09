using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using FamilyMoney.UWP.Bases;
using FamilyMoney.UWP.ViewModels;
using FamilyMoney.UWP.ViewModels.Dialogs;
using FamilyMoney.UWP.Views.Dialogs;
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
            var parameter = e.Parameter as TransactionPageParameter;
            if (e.NavigationMode == NavigationMode.New)
            {
                switch (parameter?.Action)
                {
                    case TransactionAction.CreateNewTransaction:

                    case TransactionAction.CreateTransactionForAccount:
                        ViewModel = new TransactionCreateViewModel(parameter?.ActiveAccount);
                        break;
                    case TransactionAction.EditTransaction:
                        ViewModel = new TransactionEditViewModel(parameter?.ActiveTransaction);
                        break;
                    default:
                        ViewModel = new TransactionCreateViewModel(null);
                        break;
                }

                MainPage.GlobalSettings.FormsView.Transaction = ViewModel;
            }
            else
            {
                if (MainPage.GlobalSettings.FormsView.Transaction != null)
                    ViewModel = MainPage.GlobalSettings.FormsView.Transaction;
                else
                {
                    ViewModel = new TransactionCreateViewModel(null);
                }
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
            var button = (AppBarButton) sender;
            button.Focus(FocusState.Programmatic);
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
            var childTransaction = await ViewModel.ScanChildTransaction();
            await childTransaction.ShowAsync();
        }
    }
}
