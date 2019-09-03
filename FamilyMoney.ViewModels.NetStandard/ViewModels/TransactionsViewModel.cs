using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoney.ViewModels.NetStandard.Helpers;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels
{
    public sealed class TransactionsViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<ITransaction> _transactions = new ObservableCollection<ITransaction>();
        private IAccount _activeAccount;

        public TransactionsViewModel(Storages storages)
        {
            _storages = storages;
            Accounts = new ObservableCollection<IAccount>(_storages.AccountStorage.GetAllAccounts());
            _activeAccount = Accounts.FirstOrDefault();
            RefreshTransactionByAccount();
        }

        public ObservableCollection<ITransaction> Transactions
        {
            set
            {
                _transactions = value; 
                OnPropertyChanged();
                
            }
            get => _transactions;
        }

        public ObservableCollection<IAccount> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<IAccount> _accounts;
        private readonly Storages _storages;

        public IAccount ActiveAccount
        {
            get => _activeAccount;
            set
            {
                if(_activeAccount == value) return;
                _activeAccount = value;
                OnPropertyChanged();
                RefreshTransactionByAccount();
            }
        }


        private void RefreshTransactionByAccount()
        {
            if(_activeAccount == null) return;
            Transactions.Clear();
            var accountTransactions = _storages.TransactionStorage.GetAllTransactions().Where(x => x.Account.Id == _activeAccount.Id && x.Parent == null);
            foreach (var transaction in accountTransactions)
            {
                Transactions.Add(transaction);
            }
        }


        public void DeleteTransaction(ITransaction activeTransaction)
        {
            var barCodeStorage = _storages.BarCodeStorage;

            var barCodesToDelete = barCodeStorage.GetAllBarCodes().Where(x => x.Transaction?.Id == activeTransaction.Id).ToArray();
            foreach (var barCode in barCodesToDelete)
            {
                barCodeStorage.DeleteBarCode(barCode);
            }
            _storages.TransactionStorage.DeleteTransaction(activeTransaction);
            Transactions.Remove(activeTransaction);
        }public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
