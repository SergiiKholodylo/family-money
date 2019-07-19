using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilyMoney.UWP.Annotations;
using FamilyMoneyLib.NetStandard.Bases;

namespace FamilyMoney.UWP.ViewModels.Dialogs
{
    public sealed class EditTransactionViewModel:INotifyPropertyChanged
    {
        private  IAccount _account;
        private  ICategory _category;
        private  string _name;
        private  DateTime _timestamp = DateTime.Now;
        private  decimal _total;
        private DateTimeOffset _date;
        private TimeSpan _time;
        private readonly ITransaction _transaction;

        public EditTransactionViewModel(ITransaction transaction)
        {
            _transaction = transaction;
            Date = transaction.Timestamp == DateTime.MinValue ? new DateTimeOffset(DateTime.Now) : new DateTimeOffset(transaction.Timestamp);
            Time =  transaction.Timestamp.TimeOfDay;
            Name = transaction.Name;
            Account = Accounts.FirstOrDefault(x=>x.Id == transaction.Account.Id);
            Category = Categories.FirstOrDefault(x=>x.Id == transaction.Category.Id);
            Timestamp = transaction.Timestamp;
            Total = transaction.Total;
        }

        public IAccount Account
        {
            set {
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

        public IEnumerable<ICategory> Categories { get; } = MainPage.GlobalSettings.CategoryManager.GetAllCategories();


        public IEnumerable<IAccount> Accounts { get; }= MainPage.GlobalSettings.AccountManager.GetAllAccounts();

        public void UpdateTransaction()
        {
            Timestamp = new DateTime(
                Date.Year,
                Date.Month,
                Date.Day,
                Time.Hours,
                Time.Minutes,
                Time.Seconds
            );
            var manager = MainPage.GlobalSettings.TransactionManager;
            _transaction.Name = Name;
            _transaction.Account = Account;
            _transaction.Category = Category;
            _transaction.Timestamp = Timestamp;
            _transaction.Total = Total;

            manager.UpdateTransaction(_transaction);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
