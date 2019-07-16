using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Managers;

namespace FamilyMoney.UWP.ViewModels
{
    public class TransactionsViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<ITransaction> _transactions = new ObservableCollection<ITransaction>();
        private readonly ITransactionManager _manager;
        private IAccount _activeAccount;

        public ObservableCollection<ITransaction> Transactions
        {
            set
            {
                _transactions = value; 
                OnPropertyChanged();
                OnPropertyChanged("CurrentInfo");
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

        public string CurrentInfo
        {
            get { return $"Total transactions - {Transactions.Sum(x => x.Total)}"; }
        }


        public TransactionsViewModel()
        {
            _manager = MainPage.GlobalSettings.TransactionManager;
            var accountManager = MainPage.GlobalSettings.AccountManager;
            Accounts = new ObservableCollection<IAccount>(accountManager.GetAllAccounts());
            RefreshTransactionByAccount();
        }

        internal void AddTransaction()
        {
            Transactions.Add(_manager.CreateTransaction(_activeAccount,null,"Test transaction", 120m));
        }

        public void RefreshTransactionByAccount()
        {
            var transactionManager = MainPage.GlobalSettings.TransactionManager;

            Transactions.Clear();
            var accountTransactions = transactionManager.GetAllTransactions().Where(x => x.Account == _activeAccount);
            foreach (var transaction in accountTransactions)
            {
                Transactions.Add(transaction);
            }
        }

        internal void SetActiveAccount(IAccount account)
        {
            _activeAccount = account;

            RefreshTransactionByAccount();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
