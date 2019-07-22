﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Helpers;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Managers;

namespace FamilyMoney.UWP.ViewModels
{
    public sealed class TransactionsViewModel:INotifyPropertyChanged
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
            

        get
        {
            var totalTransactions = "TotalTransactions".GetLocalized();
            return $"{totalTransactions} - {Transactions.Sum(x => x.Total)}";
        }
        }

        public IAccount ActiveAccount
        {
            get => _activeAccount;
            set
            {
                if(_activeAccount == value) return;
                _activeAccount = value;
                OnPropertyChanged();
                RefreshTransactionByAccount();
                OnPropertyChanged("CurrentInfo");
            }
        }


        public TransactionsViewModel()
        {
            _manager = MainPage.GlobalSettings.TransactionManager;
            var accountManager = MainPage.GlobalSettings.AccountManager;
            Accounts = new ObservableCollection<IAccount>(accountManager.GetAllAccounts());
            _activeAccount = Accounts.FirstOrDefault();
            RefreshTransactionByAccount();
        }

        public void RefreshTransactionByAccount()
        {
            if(_activeAccount == null) return;

            var transactionManager = MainPage.GlobalSettings.TransactionManager;

            Transactions.Clear();
            var accountTransactions = transactionManager.GetAllTransactions().Where(x => x.Account.Id == _activeAccount.Id);
            foreach (var transaction in accountTransactions)
            {
                Transactions.Add(transaction);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Refresh()
        {
            Transactions.Clear();
            var allTransactions = _manager.GetAllTransactions();
            foreach (var transaction in allTransactions)
            {
                Transactions.Add(transaction);
            }
        }

        public void DeleteTransaction(ITransaction activeTransaction)
        {
            _manager.DeleteTransaction(activeTransaction);
            Transactions.Remove(activeTransaction);
        }
    }
}
