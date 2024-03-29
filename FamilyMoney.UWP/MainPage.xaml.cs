﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Text.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.Views;
using FamilyMoney.UWP.Views.Dialogs;
using FamilyMoney.UWP.Views.Settings;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Storages;
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
        readonly MainPageViewModel _viewModel = new MainPageViewModel(GlobalSettings.Storages);

        public MainPage()
        {
            InitializeComponent();
            _viewModel.LoadQuickTransactions();

            CoreTextServicesManager textServiceManager = CoreTextServicesManager.GetForCurrentView();
            textServiceManager.InputLanguageChanged += TextServiceManager_InputLanguageChanged;
        }

        private void TextServiceManager_InputLanguageChanged(CoreTextServicesManager sender, object args)
        {
            Debug.WriteLine("Keyboard layout is changed!");
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
            var dialog = new EditQuickTransaction(null);
            var res = await dialog.ShowAsync();
        }

        private void OpenTransactionInEditMode(ITransaction transaction)
        {
            var parameters = new TransactionPageParameter(transaction, TransactionAction.CreateTransactionFromTemplate);
            Frame.Navigate(typeof(Transaction), parameters);
        }

        private static async Task ShowErrorDialog(StorageException exception)
        {
            var dialog = new ContentDialog
            {
                Content = $"Error during creating Transaction {exception.Message}",
                IsPrimaryButtonEnabled = true,
                PrimaryButtonText = "Ok"
            };
            await dialog.ShowAsync();
        }

        private async Task CreateTransactionFromTemplate(ITransaction transaction)
        {
            _viewModel.CreateTransaction(transaction);
            var dialog = new ContentDialog
            {
                Content = $"Transaction {transaction.Name} was successfully created",
                IsPrimaryButtonEnabled = true,
                PrimaryButtonText = "Ok"
            };
            await dialog.ShowAsync();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await _viewModel.ScanQuickTransaction();
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
            Frame.Navigate(typeof(Settings));
        }

        private void BtReportsButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Report));
        }
    }
}
