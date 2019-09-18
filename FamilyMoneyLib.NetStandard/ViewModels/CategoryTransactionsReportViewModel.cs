using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using FamilyMoneyLib.NetStandard.Annotations;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Reports;
using FamilyMoneyLib.NetStandard.Storages.Interfaces;

namespace FamilyMoneyLib.NetStandard.ViewModels
{
    public class CategoryTransactionsReportViewModel:INotifyPropertyChanged
    {
        private readonly IAccountStorage _accountStorage;
        private readonly ICategoryStorage _categoryStorage;
        private readonly ITransactionStorage _transactionStorage;
        private ITransactionFilteredSource _filteredSource;
        private ObservableCollection<IAccount> _accounts;
        private IAccount _account;
        private ObservableCollection<ICategory> _categories;
        private ICategory _category;
        private bool _includeSubCategories;
        private DateTimeOffset _startDate;
        private DateTimeOffset _endDate;

        public readonly ObservableCollection<ITransaction> Transactions = new ObservableCollection<ITransaction>();

        public CategoryTransactionsReportViewModel(IAccountStorage accountStorage, ICategoryStorage categoryStorage, ITransactionStorage transactionStorage)
        {
            _accountStorage = accountStorage ?? throw new ArgumentNullException(nameof(accountStorage));
            _categoryStorage = categoryStorage ?? throw new ArgumentNullException(nameof(categoryStorage)); ;
            _transactionStorage = transactionStorage ?? throw new ArgumentNullException(nameof(transactionStorage));
            _filteredSource = new TransactionFilteredSource(DateTime.Now, DateTime.Now);
            Accounts = new ObservableCollection<IAccount>(_accountStorage.GetAllAccounts());
            Categories = new ObservableCollection<ICategory>(_categoryStorage.MakeFlatCategoryTree());
            StartDate = DateTimeOffset.Now;
            EndDate = StartDate;
        }

        public void SetSource(ITransactionFilteredSource filteredSource)
        {
            _filteredSource = filteredSource;
            Account = filteredSource.Account;
            Category = filteredSource.Category;
            IncludeSubCategories = filteredSource.IncludeSubCategories;
            StartDate = filteredSource.DataStart;
            EndDate = filteredSource.DataEnd;
        }

        public void Execute()
        {
            var report = new Report2(_transactionStorage);
            var lines = report.Execute(_filteredSource);
            Transactions.Clear();
            foreach (var transaction in lines)
            {
                Transactions.Add(transaction);
            }
        }


        public ObservableCollection<IAccount> Accounts
        {
            get => _accounts;
            private set
            {
                _accounts = value;
                OnPropertyChanged();
            }
        }

        public IAccount Account
        {
            set
            {
                if (Equals(_account, value)) return;
                _account = value;
                OnPropertyChanged();
                _filteredSource.Account = _account;
                Execute();
            }
            get => _account;
        }

        public ObservableCollection<ICategory> Categories
        {
            get => _categories;
            private set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public ICategory Category
        {
            set
            {
                if (Equals(_category, value)) return;
                _category = value;
                OnPropertyChanged();
                _filteredSource.Category = _category;
                Execute();
            }
            get => _category;
        }

        public bool IncludeSubCategories
        {
            set
            {
                if(_includeSubCategories == value) return;
                _includeSubCategories = value;
                OnPropertyChanged();
                _filteredSource.IncludeSubCategories = _includeSubCategories;
                Execute();
            }
            get => _includeSubCategories;
        }

        public DateTimeOffset StartDate
        {
            set
            {
                if(_startDate == value) return;
                _startDate = value;
                OnPropertyChanged();
                _filteredSource.DataStart = _startDate.DateTime;
                Execute();
            }
            get => _startDate;
        }

        public DateTimeOffset EndDate
        {
            set
            {
                if(_endDate == value) return;
                _endDate = value;
                OnPropertyChanged();
                _filteredSource.DataEnd = _endDate.DateTime;
                Execute();
            }
            get => _endDate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
