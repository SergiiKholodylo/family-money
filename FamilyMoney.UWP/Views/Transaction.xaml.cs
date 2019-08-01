using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using FamilyMoney.UWP.ViewModels;
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

        public Transaction()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameter = e.Parameter as TransactionPageParameter;
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
        }
        private async Task EditChildTransaction(object sender)
        {
            var activeTransaction = (ITransaction)((Windows.UI.Xaml.Controls.Primitives.Selector)sender).SelectedValue;
            var editTransaction = new EditChildTransaction(ViewModel.Transaction, ViewModel.Account, activeTransaction);
            var result = await editTransaction.ShowAsync();
        }

        private void SaveTransaction()
        {
            ViewModel.SaveTransaction();
            Frame.Navigate(typeof(Transactions));
        }
        private void ExitWithoutSaving()
        {
            Frame.Navigate(typeof(Transactions));
        }

    }
}
