using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using FamilyMoney.Shared.NetStandard.ViewModels;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FamilyMoney.Shared.NetStandard.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static readonly GlobalSettings GlobalSettings = new GlobalSettings();
        readonly MainPageViewModel _viewModel = new MainPageViewModel(GlobalSettings.Storages);
        public MainPage()
        {
            InitializeComponent();
            _viewModel.LoadQuickTransactions();
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
                        OpenTransactionInEditMode(transaction);
                    }
                    else
                    {
                        try
                        {
                            await CreateTransactionFromTemplate(transaction);
                        }
                        catch (StorageException exception)
                        {
                            await ShowErrorDialog(exception);
                        }
                    }
                }
                else
                {
                    await CreateNewQuickTransaction();
                }

            }

        }

        private static async Task CreateNewQuickTransaction()
        {
            //var dialog = new EditQuickTransaction(null);
            //var res = await dialog.ShowAsync();
        }

        private void OpenTransactionInEditMode(ITransaction transaction)
        {
            //var parameters = new TransactionPageParameter(transaction, TransactionAction.CreateTransactionFromTemplate);
            //Frame.Navigate(typeof(Transaction), parameters);
        }

        private static async Task ShowErrorDialog(StorageException exception)
        {
            //var dialog = new ContentDialog
            //{
            //    Content = $"Error during creating Transaction {exception.Message}",
            //    IsPrimaryButtonEnabled = true,
            //    PrimaryButtonText = "Ok"
            //};
            //await dialog.ShowAsync();
        }

        private async Task CreateTransactionFromTemplate(ITransaction transaction)
        {
            //_viewModel.CreateTransaction(transaction);
            //var dialog = new ContentDialog
            //{
            //    Content = $"Transaction {transaction.Name} was successfully created",
            //    IsPrimaryButtonEnabled = true,
            //    PrimaryButtonText = "Ok"
            //};
            //await dialog.ShowAsync();
        }
    }
}