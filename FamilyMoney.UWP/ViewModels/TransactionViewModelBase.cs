﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using FamilyMoney.UWP.Annotations;
using FamilyMoney.UWP.Helpers;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoney.UWP.ViewModels
{
    public class TransactionViewModelBase : INotifyPropertyChanged
    {
        protected IAccount _account;
        protected ICategory _category;
        protected string _name;
        protected DateTime _timestamp = DateTime.Now;
        protected decimal _total;
        protected DateTimeOffset _date;
        protected TimeSpan _time;

        protected string _errorString;
        protected decimal _weight;
        protected bool _isComplexTransaction;
        protected ObservableCollection<ITransaction> _childrenTransactions;
        protected ITransaction _transaction;
        private Visibility _isChildTransactionVisible;

        protected TransactionViewModelBase()
        {
            Categories = MainPage.GlobalSettings.CategoryStorage.MakeFlatCategoryTree();
        }


        protected ITransaction ParentTransaction { private get; set; }

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

        protected DateTime Timestamp
        {
            set
            {
                if (_timestamp == value) return;
                _timestamp = value;
                OnPropertyChanged();
            }
            private get => _timestamp;
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
                OnPropertyChanged(nameof(IsChildTransactionVisible));
            }
            get => _isComplexTransaction;
        }

        public string ErrorString
        {
            set { _errorString = value; OnPropertyChanged(); }
            get => _errorString;
        }

        public Visibility IsChildTransactionVisible => IsComplexTransaction?Visibility.Visible:Visibility.Collapsed;

        public IEnumerable<ICategory> Categories { get; }


        public IEnumerable<IAccount> Accounts { get; } = MainPage.GlobalSettings.AccountStorage.GetAllAccounts();

        public IEnumerable<ITransaction> Transactions { get; } = MainPage.GlobalSettings.TransactionStorage.GetAllTransactions();

        public ITransaction Transaction
        {
            get => _transaction;
            set => _transaction = value;
        }

        protected void CreateTransaction()
        {
            try
            {
                ErrorString = string.Empty;
                var storage = MainPage.GlobalSettings.TransactionStorage;
                DateTimeFromDateAndTime();

                _transaction = storage.CreateTransaction(Account, Category, Name, Total, Timestamp, 0, Weight, null, ParentTransaction);
            }
            catch (StorageException e)
            {
                ErrorString = $"You have the exception {e.Message}";
            }
        }


        protected void UpdateTransaction()
        {
            try
            {
                DateTimeFromDateAndTime();
                var manager = MainPage.GlobalSettings.TransactionStorage;
                _transaction.Name = Name;
                _transaction.Account = Account;
                _transaction.Category = Category;
                _transaction.Timestamp = Timestamp;
                _transaction.Total = Total;
                _transaction.Weight = Weight;
                _transaction.Parent = ParentTransaction;

                manager.UpdateTransaction(_transaction);
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

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
