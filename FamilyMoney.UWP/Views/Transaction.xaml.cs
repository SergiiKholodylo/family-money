using System;
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
        public TransactionViewModel ViewModel;
        private Action _editTransactionAction;

        public Transaction()
        {
            this.InitializeComponent();
        }
        public Transaction(ITransaction transaction = null)
        {
            this.InitializeComponent();
            ViewModel = new TransactionViewModel(transaction);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameter = e.Parameter as TransactionPageParameter;
            ViewModel = new TransactionViewModel(parameter?.ActiveTransaction);
            if (parameter?.ActiveTransaction == null)
                _editTransactionAction = delegate { ViewModel.CreateTransaction(); };
            else
                _editTransactionAction = delegate { ViewModel.UpdateTransaction(); };
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
            _editTransactionAction();
            Frame.Navigate(typeof(Transactions));
        }

        private void CommandBar_CancelButton(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Transactions));
        }

        private async void CommandBar_AddChildTransaction(object sender, RoutedEventArgs e)
        {
            var editTransaction = new EditChildTransaction(ViewModel.Transaction, ViewModel.Account);
            var result = await editTransaction.ShowAsync();
        }

        private async void ChildrenTransaction_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var activeTransaction = (ITransaction)((Windows.UI.Xaml.Controls.Primitives.Selector)sender).SelectedValue;
            var editTransaction = new EditChildTransaction(ViewModel.Transaction, ViewModel.Account,activeTransaction);
            var result = await editTransaction.ShowAsync();
        }
    }

    public class TransactionPageParameter
    {
        public IAccount ActiveAccount;

        public TransactionPageParameter()
        {

        }

        public TransactionPageParameter(ITransaction activeTransaction)
        {
            ActiveTransaction = activeTransaction;
        }

        public TransactionPageParameter(IAccount activeAccount)
        {
            this.ActiveAccount = activeAccount;
        }

        public ITransaction ActiveTransaction { get; set; }
    }
}
