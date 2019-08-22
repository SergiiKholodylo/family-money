using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoney.ViewModels.NetStandard.Helpers;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.ViewModels.NetStandard.ViewModels
{
    public sealed class TransactionViewModel : INotifyPropertyChanged
    {
        private IAccount _account;
        private ICategory _category;
        private string _name;
        private DateTime _timestamp = DateTime.Now;
        private decimal _total;
        private DateTimeOffset _date;
        private TimeSpan _time;
        
        private string _errorString;
        private decimal _weight;
        private bool _isComplexTransaction;
        private ObservableCollection<ITransaction> _childrenTransactions;
        private ITransaction _transaction;
        private readonly Storages _storages;

        public TransactionViewModel(Storages storages,IAccount activeAccount)
        {
            _storages = storages;

            Accounts = _storages.AccountStorage.GetAllAccounts();
            Categories = storages.CategoryStorage.MakeFlatCategoryTree();
            Transactions = _storages.TransactionStorage.GetAllTransactions();

            Date = new DateTimeOffset(DateTime.Now);
            Time = DateTime.Now.TimeOfDay;
            if (activeAccount != null)
                Account = Accounts.FirstOrDefault(x => x.Id == activeAccount.Id);
        }

        public TransactionViewModel(Storages storages, ITransaction transaction)
        {
            _storages = storages;
            _transaction = transaction;

            Categories = storages.CategoryStorage.MakeFlatCategoryTree();
            Accounts = _storages.AccountStorage.GetAllAccounts();
            Transactions = _storages.TransactionStorage.GetAllTransactions();

            Date = transaction==null || transaction.Timestamp == DateTime.MinValue ? new DateTimeOffset(DateTime.Now) : new DateTimeOffset(transaction.Timestamp);
            Time = transaction?.Timestamp.TimeOfDay ?? DateTime.Now.TimeOfDay;
            Name = transaction?.Name;
            Account = Accounts.FirstOrDefault(x => x.Id == transaction?.Account.Id);
            Category = Categories.FirstOrDefault(x => x.Id == transaction?.Category.Id);
            Timestamp = transaction?.Timestamp??DateTime.Now;
            if (transaction != null)
            {
                Total = transaction.Total;
                Weight = transaction.Weight;
                IsComplexTransaction = transaction.IsComplexTransaction;
                ParentTransaction = (ITransaction)transaction.Parent;
            }

            ChildrenTransactions = transaction == null ?
                new ObservableCollection<ITransaction>() : 
                new ObservableCollection<ITransaction>(transaction.Children.Select(x=>(ITransaction)x));
        }

        public ITransaction ParentTransaction { get; set; }

        public ObservableCollection<ITransaction> ChildrenTransactions
        {
            get => _childrenTransactions;
            set => _childrenTransactions = value;
        }


        public IAccount Account
        {
            set
            {
                if (_account == value) return;
                _account = value;
                OnPropertyChanged(nameof(Accounts));
                OnPropertyChanged();
            }
            get => _account;
        }

        public ICategory Category
        {
            set
            {
                if (_category == value) return;
                _category = value;
                OnPropertyChanged(nameof(Categories));
                OnPropertyChanged();
            }
            get => _category;
        }

        public string Name
        {
            set { _name = value; OnPropertyChanged(); }
            get => _name;
        }

        public DateTime Timestamp
        {
            set
            {
                if (_timestamp == value) return;
                _timestamp = value;
                OnPropertyChanged();
            }
            get => _timestamp;
        }

        public DateTimeOffset Date
        {
            set
            {
                if (_date == value) return;
                _date = value;
                OnPropertyChanged();
            }
            get => _date;

        }

        public TimeSpan Time
        {
            set
            {
                if (_time == value) return;
                _time = value;
                OnPropertyChanged();
            }
            get => _time;

        }

        public decimal Total
        {
            set { _total = value; OnPropertyChanged(); }
            get => _total;
        }

        public decimal Weight
        {
            set { _weight = value; OnPropertyChanged(); }
            get => _weight;
        }

        public bool IsComplexTransaction
        {
            set
            {
                if(_isComplexTransaction ==  value) return;
                _isComplexTransaction = value; 
                OnPropertyChanged();
            }
            get => _isComplexTransaction;
        }

        public string ErrorString
        {
            set { _errorString = value; OnPropertyChanged(); }
            get => _errorString;
        }

        public IEnumerable<ICategory> Categories { get; }


        public readonly IEnumerable<IAccount> Accounts;

        public readonly IEnumerable<ITransaction> Transactions;

        public ITransaction Transaction
        {
            get => _transaction;
            set
            {
                if(value == _transaction) return;
                _transaction = value;
                OnPropertyChanged();
            }
        }


        public void CreateTransaction()
        {
            try
            {
                ErrorString = string.Empty;
                DateTimeFromDateAndTime();
                _transaction = _storages.TransactionStorage.CreateTransaction(Account, Category, Name, Total, Timestamp, 0, Weight, null, ParentTransaction);
            }
            catch (StorageException e)
            {
                ErrorString = $"You have the exception {e.Message}";
            }
        }


        public void UpdateTransaction()
        {
            try
            {
                DateTimeFromDateAndTime();
                _transaction.Name = Name;
                _transaction.Account = Account;
                _transaction.Category = Category;
                _transaction.Timestamp = Timestamp;
                _transaction.Total = Total;
                _transaction.Weight = Weight;
                _transaction.Parent = ParentTransaction;

                _storages.TransactionStorage.UpdateTransaction(_transaction);
            }
            catch (StorageException e)
            {
                ErrorString = $"You have the exception {e.Message}";
            }
        }
        private void DateTimeFromDateAndTime()
        {
            Timestamp = new DateTime(
                                Date.Year,
                                Date.Month,
                                Date.Day,
                                Time.Hours,
                                Time.Minutes,
                                Time.Seconds
                            );
        }
        public IEnumerable<ITransaction> GetSuggestions(string name)
        {
            return TransactionHelpers.GetSuggestions(Transactions,name);
        }

        public void FillFromTemplate(ITransaction selected)
        {
            Category = Categories.FirstOrDefault(x => x.Id == selected.Category?.Id);
            Total = selected.Total;
            Name = selected.Name;
            Weight = selected.Weight;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
