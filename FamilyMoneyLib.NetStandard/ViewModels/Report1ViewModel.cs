using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoneyLib.NetStandard.Bases;
using FamilyMoneyLib.NetStandard.Factories;
using FamilyMoneyLib.NetStandard.Reports;
using FamilyMoneyLib.NetStandard.SQLite;
using FamilyMoneyLib.NetStandard.Storages;

namespace FamilyMoneyLib.NetStandard.ViewModels
{
    public class Report1ViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<IGrouping<IAccount, KeyValuePair<Report1.CategoryAccountPair, ReportOutputValues>>> _lines = new ObservableCollection<IGrouping<IAccount, KeyValuePair<Report1.CategoryAccountPair, ReportOutputValues>>>();
        private ObservableCollection<IAccount> _accounts = new ObservableCollection<IAccount>();
        private readonly IAccountStorage _accountStorage;
        private readonly ICategoryStorage _categoryStorage;
        private readonly ITransactionStorage _transactionStorage;

        private string _header;
        private IAccount _account;

        public Report1ViewModel(IAccountStorage accountStorage,ICategoryStorage categoryStorage, ITransactionStorage transactionStorage)
        {
            _accountStorage = accountStorage ?? throw new ArgumentNullException(nameof(accountStorage));
            _categoryStorage = categoryStorage ?? throw new ArgumentNullException(nameof(categoryStorage)); ;
            _transactionStorage = transactionStorage ?? throw new ArgumentNullException(nameof(transactionStorage));

            Accounts = new ObservableCollection<IAccount>(_accountStorage.GetAllAccounts());
            Account = Accounts.FirstOrDefault();
        }

        public string Header
        {
            set
            {
                if (_header == value) return;

                _header = value;
                OnPropertyChanged();
            }
            get => _header;
        }

        public ObservableCollection<IGrouping<IAccount, KeyValuePair<Report1.CategoryAccountPair, ReportOutputValues>>> Lines
        {
            get => _lines;
            set
            {
                _lines = value; 
                OnPropertyChanged();
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
                if (_account == value) return;
                _account = value;
                OnPropertyChanged();
            }
            get => _account;
        }

        public void Execute()
        {

            var report = new Report1(_transactionStorage,_categoryStorage);
            var result = report.Execute(new AccountTransactionFilteredSource(Account)).GroupBy(x => x.Key.Account);
            var display = new ObservableCollection<IGrouping<IAccount, KeyValuePair<Report1.CategoryAccountPair, ReportOutputValues>>>(result);
            
            Lines.Clear();
            foreach (var disp in display)
            {
                Lines.Add(disp);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }

    public class Report1Structure
    {
        public IAccount Account { set; get; }
        public ICategory Category { set; get; }
        public decimal Total { set; get; }

        public string GetCategoryName => Category?.Name ?? string.Empty;

        public string GetAccountName => Category?.Name ?? string.Empty;
    }
}
