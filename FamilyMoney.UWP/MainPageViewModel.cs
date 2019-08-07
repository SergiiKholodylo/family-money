﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.Annotations;
using FamilyMoney.UWP.Bases;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP
{
    public sealed class MainPageViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<QuickButton> QuickButtons = new ObservableCollection<QuickButton>();

        public void AddButton(QuickButton button)
        {
            QuickButtons.Add(button);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task ScanQuickTransaction()
        {
            string barCodeString = await ScanBarCode();

            if (string.IsNullOrWhiteSpace(barCodeString)) return;

            var barCode = new BarCode(barCodeString);
            var storage = MainPage.GlobalSettings.BarCodeStorage;
            var transaction = storage.GetBarCodeTransaction(barCode.GetProductBarCode());

            if (transaction == null) return;

            try
            {
                CreateTransactionANdUpdateBarCode(barCode, storage, transaction);

                var dialog = new ContentDialog
                {
                    Content = $"Transaction {transaction.Name} Total {transaction.Total} was created",
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = "Ok"
                };
                await dialog.ShowAsync();
            }
            catch (StorageException exception)
            {
                var dialog = new ContentDialog
                {
                    Content = $"Error creating new transaction {transaction.Name} {exception.Message}",
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = "Ok"
                };
                await dialog.ShowAsync();

            }
        }

        private static void CreateTransactionANdUpdateBarCode(BarCode barCode, IBarCodeStorage storage, ITransaction transaction)
        {
            transaction.Timestamp = DateTime.Now;
            var transactionStorage = MainPage.GlobalSettings.TransactionStorage;
            var newTransaction = transactionStorage.CreateTransaction(transaction);
            barCode.Transaction = newTransaction;
            storage.CreateBarCode(barCode);
        }

        private static async Task<string> ScanBarCode()
        {
            var scanner = new BarCodeScanner();
            var barCodeString = await scanner.ScanBarCode();
            return barCodeString;
        }
    }

    public class QuickButton
    {
        public string Label { set; get; }
        public string Symbol { set; get; }
        public int TransactionId { set; get; }
    }
}
