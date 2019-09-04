using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using FamilyMoney.UWP.Annotations;
using FamilyMoney.UWP.Bases;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoney.UWP
{
    public sealed class MainPageViewModel:INotifyPropertyChanged
    {
        public readonly ObservableCollection<QuickButton> QuickButtons = new ObservableCollection<QuickButton>();
        private readonly Storages _storages;

        public MainPageViewModel(Storages storages)
        {
            _storages = storages;
        }

        private void AddButton(QuickButton button)
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
            var storage = _storages.BarCodeStorage;
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

        private void CreateTransactionANdUpdateBarCode(BarCode barCode, IBarCodeStorage storage, ITransaction transaction)
        {
            transaction.Timestamp = DateTime.Now;
            transaction.Parent = null;
            var transactionStorage = _storages.TransactionStorage;
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

        public void LoadQuickTransactions()
        {
            AddButton(new QuickButton
            {
                Label = "➕ Add Quick Transaction",
                TransactionId = 0
            });
            var quickTransactions = _storages.QuickTransactionStorage.GetAllQuickTransactions();
            foreach (var quickTransaction in quickTransactions)
            {
                AddButton(new QuickButton
                {
                    Label = quickTransaction.Name,
                    TransactionId = quickTransaction.Id,
                    QuickTransaction = quickTransaction
                });
            }
        }

        public void CreateTransaction(ITransaction transaction)
        {
            _storages.TransactionStorage.CreateTransaction(transaction);
        }
    }

    public class QuickButton
    {
        public string Label { set; get; }
        public long TransactionId { set; get; }
        public IQuickTransaction QuickTransaction { get; set; }


        public string MainLine => QuickTransaction == null ? Label : QuickTransaction.Name;
        public string SecondLine => QuickTransaction?.Account != null ? QuickTransaction.Account.Name : string.Empty;
        public string ThirdLine => QuickTransaction?.Category != null ? QuickTransaction.Category.Name : string.Empty;
    }
}
